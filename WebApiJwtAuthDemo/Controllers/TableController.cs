﻿using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TableController : Controller
    {
        private readonly MyRestaurantContext mContext;
        private Response response;
        public TableController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpPost]
        [ActionName("create")]
        public IActionResult AddTable([FromBody] Rtable table)
        {
            if(table == null)
            {
                return BadRequest();
            }

            Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();
            if(tmp != null)
            {
               
                response.code = 1001;
                response.message = "Location's table is used";
                response.data = null;
                return new ObjectResult(response);
            }
            else
            {
                mContext.Rtable.Add(table);
                mContext.SaveChanges();
                response.code = 1001;
                response.message = "Add table successfully !";
                response.data = table;
                return new ObjectResult(response);
            }
        }
        [HttpDelete("{location}")]
        [ActionName("delete")]
        public IActionResult Delete(string location)
        {
            var table = mContext.Rtable.FirstOrDefault(t => t.LocationTable == location);
            if (table == null)
            {
             
                response.code = 1001;
                response.message = "Delete Error, Table in location is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }

            mContext.Rtable.Remove(table);
            mContext.SaveChanges();
            response.code = 1001;
            response.message = "Delete Table Successfully !";
            response.data = table;
            return new ObjectResult(response);
          

        }
        [HttpPut]
        [ActionName("update")]
        public IActionResult ChangePro(long id, [FromBody] Rtable table)
        {
            if (table == null )
            {
                return BadRequest();
            }

            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == table.Id);
            if(tmp == null)
            {
              
                response.code = 1001;
                response.message = "Table Not Found !";
                response.data = null;
                return new ObjectResult(response);
            }

            tmp.NumberOfSeat = table.NumberOfSeat;
            tmp.Available = table.Available;
            tmp.TypeTable = table.TypeTable;
            tmp.LocationTable = table.LocationTable;
            tmp.Thumbnail = table.Thumbnail;
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();

          

            response.code = 1000;
            response.message = "Update Table Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }


    }
}
