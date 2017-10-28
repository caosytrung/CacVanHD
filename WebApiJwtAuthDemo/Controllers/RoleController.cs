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

        [HttpPost]
        [ActionName("add")]
        public IActionResult AddRole([FromBody] Role role)
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
        [HttpDelete("{location}")]
        [ActionName("delete")]
        public IActionResult Delete(string location)
        {
            var table = mContext.Rtable.FirstOrDefault(t => t.LocationTable == location);
            if (table == null)
            {
                return new ObjectResult("Delete Error ,Table in location is not exist!");
            }

            mContext.Rtable.Remove(table);
            mContext.SaveChanges();
            return new ObjectResult("Delete Table Successfully !");
        }
        [HttpPut]
        [ActionName("modify")]
        public IActionResult ChangePro(long id, [FromBody] Rtable table)
        {
            if (table == null)
            {
                return BadRequest();
            }

            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == table.Id);
            if (tmp == null)
            {
                return new ObjectResult("Table Not Found !");
            }

            tmp.NumberOfSeat = table.NumberOfSeat;
            tmp.Available = table.Available;
            tmp.TypeTable = table.TypeTable;
            tmp.LocationTable = table.LocationTable;
            tmp.Thumbnail = table.Thumbnail;
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();

            return new ObjectResult("update Table Successfully !");
        }
    }
}