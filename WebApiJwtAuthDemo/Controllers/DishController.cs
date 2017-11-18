using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyRestaurant.Controllers
{

    [Route("api/[controller]/[action]")]
    public class DishController : Controller
    {
        private readonly MyRestaurantContext mContext;
        protected Response response;
        public DishController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpGet]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult List()
        {
            Dish[] dishes = mContext.Dish.ToArray();
            for (int i = 0; i < dishes.Length; i++)
            {
                var category_id = dishes[i].CategoryId;
                var category = mContext.Category.FirstOrDefault(c => c.Id == category_id);
                if (category != null)
                {
                    dishes[i].Category = category;
                }
            }
            response.code = 1000;
            response.message = "OK";
            response.data = dishes;
            return new ObjectResult(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Get(long id)
        {
            var dish = mContext.Dish.FirstOrDefault(t => t.Id == id);
            if (dish == null)
            {
                response.code = 1001;
                response.message = "Dish not found";
                response.data = null;

            }
            else
            {
                response.code = 1000;
                response.message = "OK";
                response.data = dish;
            }

            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("create")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Create([FromForm] Dish dish)
        {

            if (dish == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
            }
            if (dish.Name == "" || dish.Name == null)
            {
                response.code = 1001;
                response.message = "Dish name must be required!";
                response.data = null;
            }
            else if (dish.Price <= 0)
            {
                response.code = 1001;
                response.message = "Price name must be required and must be positive number!";
                response.data = null;
            }
            else if (dish.CategoryId <= 0)
            {
                response.code = 1001;
                response.message = "CategoryId must be required and must be positive number!";
                response.data = null;
            }
            else
            {
                var category = mContext.Category.FirstOrDefault(t => t.Id == dish.CategoryId);
                if (category == null)
                {
                    response.code = 1001;
                    response.message = "Dont't have this category";
                    response.data = null;
                }
                else
                {
                    Dish tmp = mContext.Dish.Where(item => item.Name == dish.Name).SingleOrDefault();
                    if (tmp != null)
                    {
                        response.code = 1001;
                        response.message = "This dish has been existed!";
                        response.data = null;
                    }
                    else
                    {
                        dish.Thumbnail = dish.Thumbnail != null && dish.Thumbnail != "" ? dish.Thumbnail : "noimg.jpg";
                        mContext.Dish.Add(dish);
                        mContext.SaveChanges();
                        response.code = 1000;
                        response.message = "OK";
                        response.data = null;

                    }
                }
            }

            return new ObjectResult(response);
        }

        [HttpDelete("{id}")]
        [ActionName("delete")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Delete(int id)
        {
            var dish = mContext.Dish.FirstOrDefault(t => t.Id == id);
            if (dish == null)
            {
                response.code = 1001;
                response.message = "Fail! This dish is not existed";
                response.data = null;
            }
            else
            {
                mContext.Dish.Remove(dish);
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
        public IActionResult Update(int id, [FromForm] Dish dish)
        {
            if (dish == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
            }
            if (dish.Name == "" || dish.Name == null)
            {
                response.code = 1001;
                response.message = "Dish name must be required!";
                response.data = null;
            }
            else if (dish.Price <= 0)
            {
                response.code = 1001;
                response.message = "Price name must be required and must be positive number!";
                response.data = null;
            }
            else if (dish.CategoryId <= 0)
            {
                response.code = 1001;
                response.message = "CategoryId must be required and must be positive number!";
                response.data = null;
            }
            else
            {
                var tmp = mContext.Dish.FirstOrDefault(item => item.Id == id);
                if (tmp == null)
                {
                    response.code = 1001;
                    response.message = "Dish not found";
                    response.data = null;
                }
                else
                {
                    tmp.Name = dish.Name;
                    tmp.Price = dish.Price;
                    tmp.Thumbnail = dish.Thumbnail != null && dish.Thumbnail != "" ? dish.Thumbnail : "noimg.jpg";
                    tmp.Description = dish.Description != null ? dish.Description : "";
                    tmp.CategoryId = dish.CategoryId;
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "OK";
                    response.data = null;
                }

            }
            return new ObjectResult(response);
        }
    }
}