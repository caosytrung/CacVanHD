using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class SaleBillDetail
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int SaleBillId { get; set; }
        public int Quantity { get; set; }

        public Dish Dish { get; set; }
        public SaleBill SaleBill { get; set; }
    }
}
