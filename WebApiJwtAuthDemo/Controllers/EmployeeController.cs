using Microsoft.AspNetCore.Mvc;
using MyRestaurant.Models;
using MyRestaurant.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly MyRestaurantContext mContext;
        private Response response;
        public EmployeeController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpPost]
        [ActionName("create")]
        public IActionResult AddTable([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            Position tmp = mContext.Position.Where(item => item.Id == employee.PositionId).SingleOrDefault();
            if (tmp == null)
            {

                response.code = 1001;
                response.message = "Position Id is invalid";
                response.data = null;
                return new ObjectResult(response);
            }
            else
            {
                if(employee.Name == null || employee.PhoneNumber == null ||
                    employee.Salary == null || employee.Type == null)
                {
                    response.code = 1001;
                    response.message = "Invalid input form !";
                    response.data = null;
                    return new ObjectResult(response); 
                }
                else
                {
                    tmp.Employee.Add(employee);
                    mContext.SaveChanges();
                    response.code = 1000;
                    response.message = "Add Employee Successfully!";
                    response.data = employee;
                    return new ObjectResult(response);

                }
              
            }

        }
        [HttpDelete("{id}")]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var employee = mContext.Employee.FirstOrDefault(t => t.Id == id);
            if (employee == null)
            {

                response.code = 1001;
                response.message = "Delete Error,An Employee is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }

            mContext.Employee.Remove(employee);
            mContext.SaveChanges();
            response.code = 1000;
            response.message = "Delete Table Successfully !";
            response.data = employee;
            return new ObjectResult(response);


        }
        [HttpPut]
        [ActionName("update")]
        public IActionResult Update([FromBody] Employee employee)
        {

            if (employee == null)
            {
                response.code = 1001;
                response.message = "Invalid Parameter";
                response.data = null;
                return new ObjectResult(response);
            }



            var tmp = mContext.Employee.FirstOrDefault(item => item.Id == employee.Id);
            if (tmp == null)
            {
                response.code = 1001;
                response.message = " Employee is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }
            if (employee.Name == null || employee.PhoneNumber == null ||
                employee.PositionId == null || employee.Salary == null || employee.Type == null)
            {
                response.code = 1001;
                response.message = " Invalid Form!";
                response.data = null;
                return new ObjectResult(response);
            }

            tmp.Name = employee.Name;
            tmp.PhoneNumber = employee.PhoneNumber;
            tmp.PositionId = employee.PositionId;
            tmp.Salary = employee.Salary;
            tmp.Type = employee.Type;
            tmp.Address = employee.Address;
            mContext.Employee.Update(tmp);
            mContext.SaveChanges();



            response.code = 1000;
            response.message = "Update Employee Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }


 
    }
}
