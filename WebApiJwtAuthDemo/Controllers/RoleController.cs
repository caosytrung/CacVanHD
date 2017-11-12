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
    public class RoleController : Controller
    {
        private readonly MyRestaurantContext mContext;
        protected Response response;
        public RoleController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpGet]     
        public IActionResult List()
        {         
            Role[] roles = mContext.Role.ToArray();
            for (int i=0; i < roles.Length; i++)
            {
                int role_id = roles[i].Id;
                Users[] users = mContext.Users.Where(u => u.RoleId == role_id).ToArray();
                foreach (Users user in users)
                {
                    roles[i].Users.Add(user);
                }             
            }
            response.code = 1000;
            response.message = "OK";
            response.data = roles;
            return new ObjectResult(response);
        }

        [HttpGet("{id}")]     
        public IActionResult Get(long id)
        {
            var role = mContext.Role.FirstOrDefault(t => t.Id == id);
            if (role == null)
            {
                response.code = 1001;
                response.message = "Role not found";
                response.data = null;
              
            }
            else
            {
                Users[] users = mContext.Users.Where(u => u.RoleId == role.Id).ToArray();
                foreach (Users user in users)
                {
                    role.Users.Add(user);
                }
                response.code = 1000;
                response.message = "OK";
                response.data = role;
            }
            
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("create")]
        public IActionResult Create([FromBody] Role role)
        {
            if (role == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
            }
            else if(role.Name == "" || role.Name == null)
            {
                response.code = 1001;
                response.message = "Role name must be required!";
                response.data = null;             
            }
            else
            {
                Role tmp = mContext.Role.Where(item => item.Name == role.Name).SingleOrDefault();
                if (tmp != null)
                {
                    response.code = 1001;
                    response.message = "This role has been existed!";
                    response.data = null;                  
                }
                else
                {
                    mContext.Role.Add(role);
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "OK";
                    response.data = null;
                    
                }
            }

            return new ObjectResult(response);
        }

        [HttpDelete("{id}")]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var role = mContext.Role.FirstOrDefault(t => t.Id == id);
            if (role == null)
            {
                response.code = 1001;
                response.message = "Fail! This role is not existed";
                response.data = null;
            }
            else
            {
                mContext.Role.Remove(role);
                mContext.SaveChanges();
                response.code = 1001;
                response.message = "OK";
                response.data = null;
                
            }
            return new ObjectResult(response);
        }

        [HttpPut("{id}")]
        [ActionName("update")]
        public IActionResult Update(int id, [FromBody] Role role)
        {
            if (role == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }
            if(role.Name == "" || role.Name == null)
            {
                response.code = 1001;
                response.message = "Role name must be required";
                response.data = null;
               
            }
            else
            {
                var tmp = mContext.Role.FirstOrDefault(item => item.Id == id);
                if (tmp == null)
                {
                    response.code = 1001;
                    response.message = "Role not found";
                    response.data = null;
                }
                else
                {
                    tmp.Name = role.Name;
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