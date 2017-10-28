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
using System.Threading.Tasks;

namespace MyRestuarant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly MyRestaurantContext mContext;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        public UserController(MyRestaurantContext context, IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory)
        {
            mContext = context;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<UserController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        [HttpPost]
        [ActionName("register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (!Utils.IsValidEmail(user.Email))
            {
                return new ObjectResult("Invalid Email");
            }
            else if (user.Password.Length < 6 || user.Username.Length < 6)
            {
                return new ObjectResult("Invalid Email or Password");
            }
            int idRole = user.RoleId;

            Role role = mContext.Role.FirstOrDefault(t => t.Id == idRole);
            System.Diagnostics.Debug.WriteLine(role.Users.Count() + "  aappppp");
            bool isExistUsername = mContext.Users.Any(item => item.Username == user.Username );

            bool isExistEmail = mContext.Users.Any(item => item.Email == user.Email);
            if (isExistUsername)
            {
                return new ObjectResult("Your username is already exist !!");
            }
            if (isExistEmail)
            {
                return new ObjectResult("Your Email is already used !!");
            }
            //  return BadRequest();

            role.Users.Add(user);
            System.Diagnostics.Debug.WriteLine(role.Users.Count() + "  aappppp");
            System.Diagnostics.Debug.WriteLine("roleeeee " + role.Id);
            mContext.SaveChanges();
            try
            {
                string a = JsonConvert.SerializeObject(user, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                return new ObjectResult(a);
            }
            catch (Exception e)
            {
                return new ObjectResult("caotrung");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("login")]
        public async Task<IActionResult> Get([FromForm] ApplicationUser applicationUser)
        {
            if(applicationUser.Username == "" )
            {
                return BadRequest("Some parameters are missing");
            }
            var identity = await GetClaimsIdentity(applicationUser);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.Username}) or password ({applicationUser.Password})");
                return BadRequest("Username or password invalid");
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

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
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

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaimsIdentity(ApplicationUser user)
        {
            Users u = mContext.Users.Where(item => item.Username ==
            user.Username && item.Password == user.Password).SingleOrDefault();
            if (u != null)
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.Username, "Token"),
                  new[]
                  {
                     new Claim("DisneyCharacter", "IAmMickey")
                  }));
            }else
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }         
           
        }

       

    }
}
