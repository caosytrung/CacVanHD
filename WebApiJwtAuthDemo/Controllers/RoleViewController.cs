using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyRestaurant.Controllers
{
    public class RoleViewController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Role management";
            return View();
        }
    }
}