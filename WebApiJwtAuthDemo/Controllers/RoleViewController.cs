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
            ViewBag.Controller = "roleview";
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Role management";       
            return View();
        }

        public IActionResult Edit()
        {
            ViewBag.Title = "Role management";
            return View();
        }
    }
}