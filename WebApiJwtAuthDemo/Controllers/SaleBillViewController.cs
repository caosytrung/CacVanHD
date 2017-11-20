using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRestaurant.Controllers
{
    public class SaleBillViewController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Sale bill management";
            ViewBag.Controller = "salebillview";
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Sale bill management";
            return View();
        }

        public IActionResult Edit()
        {
            ViewBag.Title = "Sale bill management";
            return View();
        }
        public IActionResult Detail()
        {
            ViewBag.Title = "Sale bill management";
            return View();
        }
        public IActionResult CreateDetail()
        {
            ViewBag.Title = "Sale bill management";
            return View();
        }
        public IActionResult EditDetail()
        {
            ViewBag.Title = "Sale bill management";
            return View();
        }
    }
}
