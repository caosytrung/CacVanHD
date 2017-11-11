using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;

namespace MyRestaurant.Controllers
{
   
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        private readonly MyRestaurantContext mContext;
        public RoleController(MyRestaurantContext context)
        {
            mContext = context;
        }

        [HttpGet]     
        public IEnumerable<Role> List()
        {         
            Role[] arr_role = mContext.Role.ToArray();
            for (int i=0; i < arr_role.Length; i++)
            {
                int role_id = arr_role[i].Id;
                Users[] users = mContext.Users.Where(u => u.RoleId == role_id).ToArray();
                foreach (Users user in users)
                {
                    arr_role[i].Users.Add(user);
                }             
            }
            List<Role> roles = arr_role.ToList();
            return roles;
        }

        [HttpGet("{id}", Name = "GetTodo")]     
        public IActionResult Get(long id)
        {
            var role = mContext.Role.FirstOrDefault(t => t.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            Users[] users = mContext.Users.Where(u => u.RoleId == role.Id).ToArray();
            foreach (Users user in users)
            {
                role.Users.Add(user);
            }
            return new ObjectResult(role);
        }

        [HttpPost]
        [ActionName("create")]
        public IActionResult Create([FromBody] Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            if(role.Name == "")
            {
                return BadRequest("Role name must be required!");
            }

            Role tmp = mContext.Role.Where(item => item.Name == role.Name).SingleOrDefault();
            if (tmp != null)
            {
                return new ObjectResult("This role has been existed!");
            }
            else
            {
                mContext.Role.Add(role);
                mContext.SaveChanges();
                return new ObjectResult("Add role successfully!");
            }
        }

        [HttpDelete("{id}")]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var role = mContext.Role.FirstOrDefault(t => t.Id == id);
            if (role == null)
            {
                return new ObjectResult("Delete Error ,Role is not exist!");
            }

            mContext.Role.Remove(role);
            mContext.SaveChanges();
            return new ObjectResult("Delete role Successfully !");
        }
        [HttpPut("{id}")]
        [ActionName("update")]
        public IActionResult Update(long id, [FromBody] Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }

            if(role.Name == "")
            {
                return BadRequest("Role name must be required");
            }

            var tmp = mContext.Role.FirstOrDefault(item => item.Id == id);
            if (tmp == null)
            {
                return new ObjectResult("Role Not Found !");
            }

            tmp.Name = role.Name;         
            mContext.SaveChanges();

            return new ObjectResult("Update role Successfully !");
        }
    }
}