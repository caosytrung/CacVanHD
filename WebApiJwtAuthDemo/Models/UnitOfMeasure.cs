using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class UnitOfMeasure
    {
        public UnitOfMeasure()
        {
            MaterialBillDetail = new HashSet<MaterialBillDetail>();
            MaterialInStock = new HashSet<MaterialInStock>();
        }

        public int Id { get; set; }
        public string Rname { get; set; }
        public string Symbol { get; set; }
        public byte? Delected { get; set; }

        public ICollection<MaterialBillDetail> MaterialBillDetail { get; set; }
        public ICollection<MaterialInStock> MaterialInStock { get; set; }
    }
}
