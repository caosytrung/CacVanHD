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
    public class MaterialController : Controller
    {
        private readonly MyRestaurantContext mContext;
        private Response response;
        public MaterialController(MyRestaurantContext context)
        {
            mContext = context;
            response = new Response();
        }

        [HttpPost]
        [ActionName("materialinstock/create")]
        public IActionResult CreateMaterialInStock([FromBody] MaterialInStock material)
        {
            if (material == null)
            {
                return BadRequest();
            }

            // Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();
            UnitOfMeasure unitOfMeasure = mContext.UnitOfMeasure.
                Where(item => item.Id == material.UnitOfMeasureId).SingleOrDefault();
            if (unitOfMeasure == null)
            {

                response.code = 1001;
                response.message = "Invalid Unit Of Measure";
                response.data = null;
                return new ObjectResult(response);
            }
            else if (material.Price == null || material.Quatity == null || material.Name == null)
            {
                System.Diagnostics.Debug.WriteLine(material.Price + " aaaaaaaaaaaa");
                response.code = 1001;
                response.message = "Invalid Form";
                response.data = null;
                return new ObjectResult(response);


            }
            else
            {
                response.code = 1000;
                response.message = "Success to dd 1 item for material in stock !";
                response.data = material;
                mContext.MaterialInStock.Add(material);
                mContext.SaveChanges();

                return new ObjectResult(response);

            }
        }

        [HttpDelete("{id}")]
        [ActionName("materialinstock/delete")]
        public IActionResult Delete(int id)
        {
            var material = mContext.MaterialInStock.FirstOrDefault(t => t.Id == id);
            if (material == null)
            {

                response.code = 1001;
                response.message = "Delete Error,a material  is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }

            mContext.MaterialInStock.Remove(material);
            mContext.SaveChanges();
            response.code = 1000;
            response.message = "Delete Item of Material In stock Successfully !";
            response.data = material;
            return new ObjectResult(response);


        }
        [HttpPut]
        [ActionName("materialinstock/update")]
        public IActionResult UpdateMaterialInStock([FromBody] MaterialInStock material)
        {

            if (material == null)
            {
                response.code = 1001;
                response.message = "Invalid Parameter";
                response.data = null;
                return new ObjectResult(response);
            }



            var tmp = mContext.MaterialInStock.FirstOrDefault(item => item.Id == material.Id);
            var unitOM = mContext.UnitOfMeasure.FirstOrDefault(item => item.Id == material.UnitOfMeasureId);
            if (tmp == null)
            {
                response.code = 1001;
                response.message = " Material is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }
            if (unitOM == null)
            {
                response.code = 1001;
                response.message = " Material is not Found!";
                response.data = null;
                return new ObjectResult(response);
            }
            if (material.Name == null || material.Price == null ||
                material.Quatity == null || material.Description == null)
            {
                response.code = 1001;
                response.message = " Invalid Form!";
                response.data = null;
                return new ObjectResult(response);
            }

            tmp.Name = material.Name;
            tmp.Description = material.Description;
            tmp.UnitOfMeasureId = material.UnitOfMeasureId;
            tmp.Quatity = material.Quatity;
            tmp.Thumbnail = material.Thumbnail;

            mContext.MaterialInStock.Update(tmp);
            mContext.SaveChanges();



            response.code = 1000;
            response.message = "Update Material Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("materialbill/create")]
        public IActionResult CreatematerialBill([FromBody] MaterialBill materialBill)
        {
            if (materialBill == null)
            {
                return BadRequest();
            }

            // Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();
            Employee employeeRespond = mContext.Employee.
                Where(item => item.Id == materialBill.EmployeeRespond).SingleOrDefault();
            if (employeeRespond == null)
            {

                response.code = 1001;
                response.message = "Invalid Employee ID ";
                response.data = null;
                return new ObjectResult(response);
            }

            if (materialBill.Total == null)
            {
                //     System.Diagnostics.Debug.WriteLine(material.Price + " aaaaaaaaaaaa");
                response.code = 1001;
                response.message = "Invalid Form";
                response.data = null;
                return new ObjectResult(response);


            }
            else
            {
                response.code = 1000;
                response.message = "Success to add a bill for materialBill !";
                response.data = materialBill;
                mContext.MaterialBill.Add(materialBill);
                mContext.SaveChanges();

                return new ObjectResult(response);

            }
        }
        [HttpPut]
        [ActionName("materialbill/update")]
        public IActionResult UpdateMaterialBill([FromBody] MaterialBill materialBill)
        {

            if (materialBill == null)
            {
                response.code = 1001;
                response.message = "Invalid Parameter";
                response.data = null;
                return new ObjectResult(response);
            }



            var tmp = mContext.MaterialBill.FirstOrDefault(item => item.Id == materialBill.Id);
            var employeeRespond = mContext.Employee.FirstOrDefault(item => item.Id == materialBill.EmployeeRespond);
            if (tmp == null)
            {
                response.code = 1001;
                response.message = " MaterialBill is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }
            if (employeeRespond == null)
            {
                response.code = 1001;
                response.message = " Employee respond is not Found!";
                response.data = null;
                return new ObjectResult(response);
            }
            if (materialBill.Total == null
               )
            {
                response.code = 1001;
                response.message = " Invalid Form!";
                response.data = null;
                return new ObjectResult(response);
            }

            tmp.Total = materialBill.Total;
            tmp.EmployeeRespond = materialBill.EmployeeRespond;

            mContext.MaterialBill.Update(tmp);
            mContext.SaveChanges();



            response.code = 1000;
            response.message = "Update Material Bill Successfully !";
            response.data = tmp;
            return new ObjectResult(response);
        }

        [HttpDelete("{id}")]
        [ActionName("materialbill/delete")]
        public IActionResult DeleteMaterialBill(int id)
        {
            var material = mContext.MaterialBill.FirstOrDefault(t => t.Id == id);
            if (material == null)
            {

                response.code = 1001;
                response.message = "Delete Error,a material Bill  is not exist!";
                response.data = null;
                return new ObjectResult(response);
            }

            mContext.MaterialBill.Remove(material);
            mContext.SaveChanges();
            response.code = 1000;
            response.message = "Delete Item of Material Bill Successfully !";
            response.data = material;
            return new ObjectResult(response);


        }

        [HttpPost]
        [ActionName("materialbilldetail/create")]
        public IActionResult CreateMaterialBillDetail([FromBody] MaterialBillDetail materialBillDetail)
        {
            if (materialBillDetail == null)
            {
                return BadRequest();
            }
            MaterialBill materialBill = mContext.MaterialBill.SingleOrDefault(item => item.Id ==
            materialBillDetail.MaterialBillId);



            // Rtable tmp = mContext.Rtable.Where(item => item.LocationTable == table.LocationTable).SingleOrDefault();

            if (materialBill == null)
            {

                response.code = 1001;
                response.message = "Invalid Material bill ID";
                response.data = null;
                return new ObjectResult(response);
            }
            MaterialInStock materialInStock = mContext.MaterialInStock.SingleOrDefault(item => item.Id ==
          materialBillDetail.MaterialId);
            if (materialBill == null)
            {

                response.code = 1001;
                response.message = "Invalid Material In Stock  ID";
                response.data = null;
                return new ObjectResult(response);
            }

            if (materialBillDetail.Quantity == null)
            {

                response.code = 1001;
                response.message = "Invalid Form";
                response.data = null;
                return new ObjectResult(response);


            }
            else
            {
                response.code = 1000;
                response.message = "Success to add a bill detail !";
                response.data = materialBillDetail;
                mContext.MaterialBillDetail.Add(materialBillDetail);
                materialInStock.Quatity = materialInStock.Quatity + materialBillDetail.Quantity;
                mContext.MaterialInStock.Update(materialInStock);

                mContext.SaveChanges();

                return new ObjectResult(response);

            }
        }

        [HttpGet]
        [ActionName("list")]
        public IActionResult List()
        {
            var employees = mContext.MaterialInStock.ToList();
            if (employees.Count() == 0)
            {
                response.setDatas(1001, "Material  is empty set !", null);
                return new ObjectResult(response);
            }
            response.setDatas(1000, "Query successfully !!", employees);

            return new ObjectResult(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            System.Diagnostics.Debug.Write("Davaoroiii");
            //if (!id.HasValue) {
            //    response.setDatas(1001, "Please pass value for get data !", null);
            //    return new ObjectResult(response);
            //}
            var material = mContext.MaterialInStock.FirstOrDefault(item => item.Id == id);
            if (material == null)
            {
                response.setDatas(1001, "No material valid !", null);
                return new ObjectResult(response);
            }
            response.setDatas(1000, "Query Success !", material);
            return new ObjectResult(response);


        }

    }
}
