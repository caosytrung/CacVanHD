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

        [HttpGet]
        [Authorize(Policy = "DisneyUser")]
        public IActionResult List()
        {
            Rtable[] dishes = mContext.Rtable.ToArray();
            
            response.code = 1000;
            response.message = "OK";
            response.data = dishes;
            return new ObjectResult(response);
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
<<<<<<< HEAD
        public IActionResult Update(long id, [FromBody] Rtable table)
=======
        [Authorize(Policy = "DisneyUser")]
        public IActionResult Update(long id, [FromForm] Rtable table)
>>>>>>> 445132923790bddf6641eb1b93f133bd6d0e51cf
        {
            if (table == null)
            {
                response.code = 1001;
                response.message = "Input is null";
                response.data = null;
                return new ObjectResult(response);
            }

<<<<<<< HEAD
            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == table.Id);
            if (tmp == null)
=======
            var tmp = mContext.Rtable.FirstOrDefault(item => item.Id == id);
            if(tmp == null)
>>>>>>> 445132923790bddf6641eb1b93f133bd6d0e51cf
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
        [ActionName("listtable")]
        public IActionResult List()
        {
            System.Diagnostics.Debug.Write("roiiii");
            var employees = mContext.Rtable.ToList();
            if (employees.Count() == 0)
            {
                response.setDatas(1001, "Table  is empty set !", null);
                return new ObjectResult(response);
            }
            response.setDatas(1000, "Query successfully !!", employees);

            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("tablebooked")]
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
        public IActionResult BookTable(long? id)
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
            response.setDatas(1000, "Table is booked successfully !", tmp);
            tmp.Available = 0;
            mContext.Rtable.Update(tmp);
            mContext.SaveChanges();

            return new ObjectResult(response);


        }

    }
}
