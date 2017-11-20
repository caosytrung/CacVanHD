using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRestaurant.Controllers
{
    public class EmployeeViewController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Title = "Employee management";
            ViewBag.Controller = "employeeview";
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Employee management";
            return View();
        }

        public IActionResult Edit()
        {
            ViewBag.Title = "Employee management";
            return View();
        }
    }
}
