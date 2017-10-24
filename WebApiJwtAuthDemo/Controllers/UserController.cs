using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly MyRestaurantContext mContex;
        public UserController(MyRestaurantContext Context)
        {
            mContex = Context;
        }

        [HttpPost]
        public  IActionResult Register([FromBody] ConfigDefault config )
        {
            //mContex.Add(user);
            //await mContex.SaveChangesAsync();

            return new ObjectResult(config);
        }

    }
}
