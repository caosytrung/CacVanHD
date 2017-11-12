using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyRestaurant;
using MyRestaurant.Models;
using MyRestaurant.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyRestuarant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        protected readonly MyRestaurantContext mContext;
        protected readonly JwtIssuerOptions _jwtOptions;
        protected readonly ILogger _logger;
        protected readonly JsonSerializerSettings _serializerSettings;
        protected Response response;

        public UserController(MyRestaurantContext context, IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory)
        {
            mContext = context;
            response = new Response();
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<UserController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            response = new Response();
        }
        [HttpPost]      
        public IActionResult Register([FromBody] Users user)
        {
            if (!Utils.IsValidEmail(user.Email))
            {


                response.code = 1001;
                response.message = "Invalid Email";
                response.data = null;
                return new ObjectResult(response);
            }
            else if (user.Password.Length < 6 || user.Username.Length < 4)
            {
                response.code = 1001;
                response.message = "Email or password is incorrect";
                response.data = null;
                return new ObjectResult(response);
            }           
           
            bool isExistUsername = mContext.Users.Any(item => item.Username == user.Username );

            bool isExistEmail = mContext.Users.Any(item => item.Email == user.Email);
            if (isExistUsername)
            {
                
                response.code = 1001;
                response.message = "Your username is already exist";
                response.data = null;
                return new ObjectResult(response);
            }
            if (isExistEmail)
            {
             
                response.code = 1001;
                response.message = "Your Email is already used";
                response.data = null;
                return new ObjectResult(response);
            }
            
            string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));
            user.Password = password;
            user.Avatar = "noimg.jpg";          
          
            mContext.Users.Add(user);         
            mContext.SaveChanges();
            try
            {
                string a = JsonConvert.SerializeObject(user, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                response.code = 1000;
                response.message = "OK";
                response.data = user;
                return new ObjectResult(response);
            }
            catch (Exception e)
            {
                response.code = 1001;
                response.message = "Error";
                response.data = null;
                return new ObjectResult(response);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("login")]
        public async Task<IActionResult> Get([FromForm] ApplicationUser applicationUser)
        {
            Response res = new Response();
            if(applicationUser.Username == "" )
            {
                res.code = 1001;
                res.message = "Some parameters are missing";
                res.data = null;
                return new ObjectResult(res);
            }
            var identity = await GetClaimsIdentity(applicationUser);
            if (identity == null)
            {
                res.code = 1001;
                res.message = "Username or password invalid";
                res.data = null;
                return new ObjectResult(res);
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("DisneyCharacter")
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new
            {               
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };
            res.code = 1000;
            res.message = "OK";
            res.data = response;

            //var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new ObjectResult(res);
        }

        public IActionResult List()
        {
            Users[] users = mContext.Users.ToArray();
            for (int i = 0; i < users.Length; i++)
            {
                int role_id = users[i].RoleId;
                Role role = mContext.Role.FirstOrDefault(r => r.Id == role_id);
                users[i].Role = role;
            }
            response.code = 1000;
            response.message = "OK";
            response.data = users;
            return new ObjectResult(response);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaimsIdentity(ApplicationUser user)
        {
            Users u = mContext.Users.Where(item => item.Username ==
            user.Username).SingleOrDefault();
            if (u != null)
            {
                string password = Encoding.UTF8.GetString(Convert.FromBase64String(u.Password));
                if(password == user.Password)
                {
                    return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.Username, "Token"),
                 new[]
                 {
                     new Claim("DisneyCharacter", "IAmMickey")
                 }));
                }
                else
                {
                    return Task.FromResult<ClaimsIdentity>(null);
                }
               
            }else
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }         
           
        }


       

    }
}
