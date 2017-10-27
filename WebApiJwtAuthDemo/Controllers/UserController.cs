using Microsoft.AspNetCore.Mvc;
using MyRestaurant;
using MyRestaurant.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestuarant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly MyRestaurantContext mContext;
        public UserController(MyRestaurantContext context)
        {
            mContext = context;

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
        [ActionName("login")]
        public IActionResult Login([FromBody] PostLogin user)
        {
            Users u = mContext.Users.Where(item => item.Username ==
            user.Username && item.Password == user.Password).SingleOrDefault();
            

            if(u != null)
            {
                return new ObjectResult("Login Success !");
            }
            return new ObjectResult("Username or Password inn't correct, Please Try againt !");
        }


        public class PostLogin
        {
            public String Username { get; set; }
            public String Password { get; set; }
        }

    }
}
