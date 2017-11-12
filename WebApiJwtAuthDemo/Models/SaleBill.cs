using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class SaleBill
    {
        public SaleBill()
        {
            SaleBillDetail = new HashSet<SaleBillDetail>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int EmployeeId { get; set; }
        public byte[] CreatAt { get; set; }
        public decimal Total { get; set; }

        public Employee Employee { get; set; }
        public ICollection<SaleBillDetail> SaleBillDetail { get; set; }
    }
}
