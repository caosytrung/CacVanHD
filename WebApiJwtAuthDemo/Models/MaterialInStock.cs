using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class MaterialInStock
    {
        public MaterialInStock()
        {
            MaterialBillDetail = new HashSet<MaterialBillDetail>();
        }

        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public decimal? Quatity { get; set; }
        public int UnitOfMeasureId { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ICollection<MaterialBillDetail> MaterialBillDetail { get; set; }
    }
}
