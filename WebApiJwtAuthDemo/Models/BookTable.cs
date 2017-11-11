using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class BookTable
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookAt { get; set; }

        public Rtable Table { get; set; }
    }
}
