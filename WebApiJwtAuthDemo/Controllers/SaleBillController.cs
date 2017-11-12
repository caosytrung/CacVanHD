using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;

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
        public IActionResult List()
        {
            SaleBill[] saleBill = mContext.SaleBill.ToArray();
            for (int i = 0; i < saleBill.Length; i++)
            {
                int saleBillId = saleBill[i].Id;
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(u => u.SaleBillId == saleBillId).ToArray();
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
                SaleBillDetail[] saleBillDetails = mContext.SaleBillDetail.Where(u => u.SaleBillId == saleBill.Id).ToArray();
                foreach (SaleBillDetail saleBillDetail in saleBillDetails)
                {
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
        public IActionResult Create([FromBody] SaleBill saleBill)
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

        [HttpPost]      
        public IActionResult CreateDetail([FromBody] SaleBillDetail saleBillDetail)
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

        //[HttpPut]
        //public IActionResult UpdateDetailQuantity(int id, [FromBody] SaleBillDetail sale)
        //{
        //    var saleBillDetail = mContext.SaleBillDetail.FirstOrDefault(t => t.Id == id);
        //    if(saleBillDetail == null)
        //    {
        //        response.code = 1001;
        //        response.message = "Bill detail not found";
        //        response.data = null;
        //    }
        //    else
        //    {
        //        saleBillDetail.Quantity = sale.Quantity;
        //        mContext.SaveChanges();
        //        var saleBill = mContext.SaleBill.FirstOrDefault(t => t.Id == saleBillDetail.SaleBillId);
        //        var dish = mContext.Dish.FirstOrDefault(t => t.Id == saleBillDetail.DishId);
        //        saleBill.Total = dish.Price * sale.Quantity;
        //        mContext.SaveChanges();
        //        response.code = 1000;
        //        response.message = "OK";
        //        response.data = null;
        //    }
            
        //    return new ObjectResult(response);
        //}
    }

}