using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{id}")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Get(long id)
        {
            var table = mContext.Rtable.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                response.code = 1001;
                response.message = "Table not found";
                response.data = null;

            }
            else
            {
                response.code = 1000;
                response.message = "OK";
                response.data = table;
            }

            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("create")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Create([FromForm] Rtable table)
        {
            if (table == null)
            {
                return BadRequest();
            }

            Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();
            if (tmp != null)
            {

                response.code = 1001;
                response.message = "Location's table is used";
                response.data = null;
                return new ObjectResult(response);
            }
            else
            {
                table.Available = 1;
                table.Thumbnail = "noimg.jpg";
                mContext.Rtable.Add(table);
                mContext.SaveChanges();
                response.code = 1000;
                response.message = "Add table successfully !";
                response.data = table;
                return new ObjectResult(response);
            }
        }
        [HttpDelete("{id}")]
        [ActionName("delete")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Delete(int id)
        {
            var table = mContext.Rtable.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {

                response.code = 1001;
                response.message = "Delete Error, Table is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }

            mContext.Rtable.Remove(table);
            mContext.SaveChanges();
            response.code = 1000;
            response.message = "Delete Table Successfully !";
            response.data = table;
            return new ObjectResult(response);


        }
        [HttpPut("{id}")]
        [ActionName("update")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Update(long id, [FromForm] Rtable table)
        {
            if (table == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }

            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == id);
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
            if(table.Thumbnail != null && table.Thumbnail != "")
            {
                tmp.Thumbnail =   table.Thumbnail;
            }
           
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();



            response.code = 1000;
            response.message = "Update Table Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("list")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult List()
        {
          
            var tables = mContext.Rtable.ToList();
            if (tables.Count() == 0)
            {
                response.setDatas(1001, "Table  is empty set !", null);
                return new ObjectResult(response);
            }
            response.setDatas(1000, "Query successfully !!", tables);

            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("tablebooked")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult ListTableBooked()
        {
            System.Diagnostics.Debug.Write("roiiii");
            var tables = mContext.Rtable.Where(item => item.Available == 1).ToList();
            if (tables.Count() == 0)
            {
                response.setDatas(1001, "Table booked  is empty set !", null);
                return new ObjectResult(response);
            }
            response.setDatas(1000, "Query successfully !!", tables);

            return new ObjectResult(response);
        }
        [HttpPut("{id}")]
        [ActionName("booktable")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult BookTable(long? id, [FromForm] BookTable bookTable)
        {
            if (!id.HasValue)
            {
                response.setDatas(1001, "Please put id !", null);
                return new ObjectResult(response);
            }

            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == id);
            if (tmp == null)
            {
                response.setDatas(1001, "Table not found !", null);
                return new ObjectResult(response);
            }

            if (tmp.Available == 0)
            {
                response.setDatas(1001, "book table failed , this table is booked yet !", null);
                return new ObjectResult(response);
            }

            tmp.Available = 0;
            
            if(bookTable.CustomerName == null || bookTable.CustomerName == "")
            {
                response.setDatas(1000, "Customer name must be required!", tmp);
            }

            if (bookTable.CustomerPhone == null || bookTable.CustomerPhone == "")
            {
                response.setDatas(1000, "Customer phone must be required!", tmp);
            }

            if(bookTable.BookAt == null)
            {
                response.setDatas(1000, "Book time must be required!", tmp);
            }
            mContext.BookTable.Add(bookTable);
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();
            response.setDatas(1000, "Table is booked successfully !", tmp);
            return new ObjectResult(response);


        }
        [HttpPut("{id}")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Return(long? id)
        {
            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == id);
            if (tmp == null)
            {
                response.setDatas(1001, "Table not found !", null);
                return new ObjectResult(response);
            }
           
            response.setDatas(1000, "Table is booked successfully !", tmp);
            tmp.Available = 1;
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();

            return new ObjectResult(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult GetCustomer(long id)
        {
            var tmp = mContext.BookTable.FirstOrDefault(item => item.TableId == id);
            if (tmp == null)
            {
                response.setDatas(1001, "Table not found !", null);
                return new ObjectResult(response);
            }

            response.setDatas(1000, "Table is booked successfully !", tmp);       
            return new ObjectResult(response);
        }


    }
}
