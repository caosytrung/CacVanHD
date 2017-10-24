using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class MaterialBillDetail
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public int MaterialBillId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public decimal Quantity { get; set; }

        public MaterialInStock Material { get; set; }
        public MaterialBill MaterialBill { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
