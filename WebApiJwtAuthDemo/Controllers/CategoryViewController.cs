using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRestaurant.Controllers
{
    public class CategoryViewController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Category management";
            ViewBag.Controller = "categoryview";
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Category management";
            return View();
        }

        public IActionResult Edit()
        {
            ViewBag.Title = "Category management";
            return View();
        }
    }
}
