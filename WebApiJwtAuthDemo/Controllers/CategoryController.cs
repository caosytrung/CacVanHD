using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {

        private readonly MyRestaurantContext mContext;
        public CategoryController(MyRestaurantContext context)
        {
            mContext = context;
        }
     
        [HttpGet]
        public IEnumerable<Category> List()
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
            return categories.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = mContext.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            Dish[] dishes = mContext.Dish.Where(d => d.CategoryId == category.Id).ToArray();
            foreach (Dish dish in dishes)
            {
                category.Dish.Add(dish);
            }
            return new ObjectResult(category);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody]Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            if (category.Name == "")
            {
                return BadRequest("Category name must be required!");
            }

            Role tmp = mContext.Role.Where(item => item.Name == category.Name).SingleOrDefault();
            if (tmp != null)
            {
                return new ObjectResult("This category has been existed!");
            }
            else
            {              
                mContext.Category.Add(category);
                mContext.SaveChanges();
                return new ObjectResult("Add category successfully!");
            }
        }

        [HttpDelete("{id}")]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var category = mContext.Category.FirstOrDefault(t => t.Id == id);
            if (category == null)
            {
                return new ObjectResult("Delete Error ,Role is not exist!");
            }

            mContext.Category.Remove(category);
            mContext.SaveChanges();
            return new ObjectResult("Delete role Successfully !");
        }
        [HttpPut("{id}")]
        [ActionName("update")]
        public IActionResult Update(long id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            if (category.Name == "")
            {
                return BadRequest("Category name must be required");
            }

            var tmp = mContext.Role.FirstOrDefault(item => item.Id == id);
            if (tmp == null)
            {
                return new ObjectResult("Category Not Found !");
            }

            tmp.Name = category.Name;
            mContext.SaveChanges();

            return new ObjectResult("Update role Successfully !");
        }
    }
}
