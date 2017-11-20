using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;
using Microsoft.AspNetCore.Authorization;

namespace MyRestaurant.Controllers
{

    [Route("api/[controller]/[action]")]
    public class SaleBillController : Controller
    {
        private readonly MyRestaurantContext mContext;
        protected Response response;
        public SaleBillController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpGet]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult List()
        {
            SaleBill[] saleBill = mContext.SaleBill.ToArray();
            for (int i = 0; i < saleBill.Length; i++)
            {
                int saleBillId = saleBill[i].Id;
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(u => u.SaleBillId == saleBillId).ToArray();
                Employee employee = mContext.Employee.FirstOrDefault(t => t.Id == saleBill[i].EmployeeId);
                saleBill[i].Employee = employee;
                foreach (SaleBillDetail saleBillDetail in saleBillDetails)
                {                 
                    saleBill[i].SaleBillDetail.Add(saleBillDetail);
                }
            }
            response.code = 1000;
            response.message = "OK";
            response.data = saleBill;
            return new ObjectResult(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Get(long id)
        {
            var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == id);
            if (saleBill == null)
            {
                response.code = 1001;
                response.message = "Bill not found";
                response.data = null;

            }
            else
            {
                Employee employee = mContext.Employee.FirstOrDefault(t => t.Id == saleBill.EmployeeId);
                saleBill.Employee = employee;
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(u => u.SaleBillId == saleBill.Id).ToArray();
                foreach (SaleBillDetail saleBillDetail in saleBillDetails)
                {
                    Dish dish = mContext.Dish.FirstOrDefault(t => t.Id == saleBillDetail.DishId);
                    saleBillDetail.Dish = dish;
                    saleBill.SaleBillDetail.Add(saleBillDetail);
                }
                response.code = 1000;
                response.message = "OK";
                response.data = saleBill;
            }

            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("create")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Create([FromForm] SaleBill saleBill)
        {
            if (saleBill == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
            }
            else if (saleBill.CustomerName == "" || saleBill.CustomerName == null)
            {
                response.code = 1001;
                response.message = "Customer's name must be required!";
                response.data = null;
            }
            else if (saleBill.EmployeeId <= 0)
            {
                response.code = 1001;
                response.message = "Employee invalid";
                response.data = null;
            }
            else
            {
                decimal total = 0;
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(u => u.SaleBillId == saleBill.Id).ToArray();
                foreach (SaleBillDetail saleBillDetail in saleBillDetails)
                {
                    total += saleBillDetail.Quantity * saleBillDetail.Dish.Price;
                }
                saleBill.Total = total;

                mContext.SaleBill.Add(saleBill);
                mContext.SaveChanges();
                response.code = 1000;
                response.message = "OK";
                response.data = null;
            }
            return new ObjectResult(response);
        }

        [HttpPut("{id}")]
        [ActionName("update")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Update(long id, [FromForm] SaleBill salebill)
        {
            if (salebill == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }

            var tmp = mContext.SaleBill.FirstOrDefault(item => item.Id == id);
            if (tmp == null)
            {

                response.code = 1001;
                response.message = "Bill Not Found !";
                response.data = null;
                return new ObjectResult(response);
            }
            if(salebill.CustomerName != "" && salebill.CustomerName != null)
            {
                tmp.CustomerName = salebill.CustomerName;
            }

            if (salebill.CreatAt != null)
            {
                tmp.CreatAt = salebill.CreatAt;
            }

            mContext.SaleBill.Update(tmp);
            mContext.SaveChanges();

            response.code = 1000;
            response.message = "Update Bill Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }

        [HttpPost]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult CreateDetail([FromForm] SaleBillDetail saleBillDetail)
        {
            if (saleBillDetail == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
            }
            else
            {
                if (saleBillDetail.DishId <= 0 || saleBillDetail.SaleBillId <= 0 || saleBillDetail.Quantity <= 0)
                {
                    response.code = 1001;
                    response.message = "Param not enough or invalid";
                    response.data = null;
                }
                else
                {
                    var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == saleBillDetail.SaleBillId);
                    var dish = mContext.Dish.FirstOrDefault(t => t.Id == saleBillDetail.DishId);
                    decimal total = saleBillDetail.Quantity * dish.Price;
                    total += saleBill.Total;
                    saleBill.Total = total;

                    var tmp = mContext.SaleBillDetail.Where(s => s.SaleBillId == saleBillDetail.SaleBillId).
                        Where(s => s.DishId == saleBillDetail.DishId).SingleOrDefault();
                    if(tmp != null)
                    {
                        tmp.Quantity += saleBillDetail.Quantity;
                    }
                    else
                    {
                        mContext.SaleBillDetail.Add(saleBillDetail);
                    }               
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "OK";
                    response.data = null;
                }
            }

            return new ObjectResult(response);
        }

        [HttpDelete("{id}")]     
        public IActionResult Delete(int id)
        {
            var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == id);
            if (saleBill == null)
            {
                response.code = 1001;
                response.message = "Fail! This bill is not existed";
                response.data = null;
            }
            else
            {
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(s => s.SaleBillId == id).ToArray();
                foreach(SaleBillDetail s in saleBillDetails)
                {
                    mContext.Remove(s);
                    mContext.SaveChanges();
                }
                mContext.SaleBill.Remove(saleBill);
                mContext.SaveChanges();
                response.code = 1000;
                response.message = "OK";
                response.data = null;

            }
            return new ObjectResult(response);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveDetail(int id)
        {
            var saleBillDetail = mContext.SaleBillDetail.FirstOrDefault(t => t.Id == id);
            if (saleBillDetail == null)
            {
                response.code = 1001;
                response.message = "Bill detail not found";
                response.data = null;
            }
            else
            {
                var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == saleBillDetail.SaleBillId);
                var dish = mContext.Dish.FirstOrDefault(t => t.Id == saleBillDetail.DishId);
                decimal total = saleBill.Total - saleBillDetail.Quantity * dish.Price;
                saleBill.Total = total;
                mContext.SaleBillDetail.Remove(saleBillDetail);
                mContext.SaveChanges();
                response.code = 1000;
                response.message = "OK";
                response.data = null;

            }
            return new ObjectResult(response);
        }

        [HttpPost]
        public IActionResult UpdateDetailQuantity([FromForm] SaleBillDetail sale)
        {
            var saleBillDetail = mContext.SaleBillDetail.FirstOrDefault(t => t.Id == sale.Id);
            int quantity = saleBillDetail.Quantity;
            if (saleBillDetail == null)
            {
                response.code = 1001;
                response.message = "Bill detail not found";
                response.data = null;
            }
            else
            {
                saleBillDetail.Quantity = sale.Quantity;
                mContext.SaveChanges();
                var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == saleBillDetail.SaleBillId);
                var dish = mContext.Dish.FirstOrDefault(t => t.Id == saleBillDetail.DishId);
                saleBill.Total += dish.Price * (sale.Quantity - quantity);
                mContext.SaveChanges();
                response.code = 1000;
                response.message = "OK";
                response.data = sale.Quantity * dish.Price;
            }

            return new ObjectResult(response);
        }
    }

}