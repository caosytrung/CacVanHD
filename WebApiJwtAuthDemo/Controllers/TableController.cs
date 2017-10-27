using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TableController : Controller
    {
        private readonly MyRestaurantContext mContext;
        public TableController(MyRestaurantContext context)
        {
            mContext = context;
        }

        [HttpPost]
        [ActionName("add")]
        public IActionResult AddTable([FromBody] Rtable table)
        {
            if(table == null)
            {
                return BadRequest();
            }

            Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();
            if(tmp != null)
            {
                return new ObjectResult("Location's table is used !");
            }
            else
            {
                mContext.Rtable.Add(table);
                mContext.SaveChanges();
                return new ObjectResult("Add Table successfully !" );
            }
        }
        [HttpDelete("{location}")]
        [ActionName("delete")]
        public IActionResult Delete(string location)
        {
            var table = mContext.Rtable.FirstOrDefault(t => t.LocationTable == location);
            if (table == null)
            {
                return new ObjectResult("Delete Error ,Table in location is not exist!" );
            }

            mContext.Rtable.Remove(table);
            mContext.SaveChanges();
            return new ObjectResult("Delete Table Successfully !");
        }
        [HttpPut]
        [ActionName("modify")]
        public IActionResult ChangePro(long id, [FromBody] Rtable table)
        {
            if (table == null )
            {
                return BadRequest();
            }

            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == table.Id);
            if(tmp == null)
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
