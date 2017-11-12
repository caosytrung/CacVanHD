using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {

        private readonly MyRestaurantContext mContext;
        protected Response response;
        public CategoryController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }
     
        [HttpGet]
        public IActionResult List()
        {
            Category[] categories = mContext.Category.ToArray();
            for(int i=0; i < categories.Length; i++)
            {
                int caterory_id = categories[i].Id;
                Dish[] dishes = mContext.Dish.Where(d => d.CategoryId == caterory_id).ToArray();
                foreach (Dish dish in dishes)
                {
                    categories[i].Dish.Add(dish);
                }
            }
            response.code = 1000;
            response.message = "OK";
            response.data = categories;       
            return new ObjectResult(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = mContext.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                response.code = 1001;
                response.message = "Category not found";
                response.data = null;
            }
            else
            {
                Dish[] dishes = mContext.Dish.Where(d => d.CategoryId == category.Id).ToArray();
                foreach (Dish dish in dishes)
                {
                    category.Dish.Add(dish);
                }
                response.code = 1000;
                response.message = "OK";
                response.data = category;
            }
           
            return new ObjectResult(response);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody]Category category)
        {
            if (category == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }
            else if (category.Name == "" || category.Name == null)
            {
                response.code = 1001;
                response.message = "Category name must be required";
                response.data = null;
                return new ObjectResult(response);
            }
            else
            {
                Category tmp = mContext.Category.Where(item => item.Name == category.Name).SingleOrDefault();
                if (tmp != null)
                {
                    response.code = 1001;
                    response.message = "Category has been existed";
                    response.data = null;
                    return new ObjectResult(response);
                }
                else
                {
                    mContext.Category.Add(category);
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "OK";
                    response.data = null;
                    return new ObjectResult(response);
                }
            }
          

        }

        [HttpDelete("{id}")]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var category = mContext.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                response.code = 1001;
                response.message = "Fail! Category is not existed";
                response.data = null;
                return new ObjectResult(response);
            }
           
            mContext.Category.Remove(category);
            mContext.SaveChanges();
            response.code = 1000;
            response.message = "OK";
            response.data = null;
            return new ObjectResult(response);                           
        }

        [HttpPut("{id}")]
        [ActionName("update")]
        public IActionResult Update(long id, [FromBody] Category category)
        {
            if (category == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }
            else if (category.Name == "")
            {
                response.code = 1001;
                response.message = "Category name must be required";
                response.data = null;
                return new ObjectResult(response);
            }
            else
            {
                var tmp = mContext.Category.FirstOrDefault(item => item.Id == id);
                if (tmp == null)
                {
                    response.code = 1001;
                    response.message = "Category Not Found !";
                    response.data = null;
                    return new ObjectResult(response);
                }
                else
                {
                    tmp.Name = category.Name;
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "OK";
                    response.data = null;
                    return new ObjectResult(response);
                }

            }
        }
    }
}
