using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Employee
    {
        public Employee()
        {
            MaterialBill = new HashSet<MaterialBill>();
            SaleBill = new HashSet<SaleBill>();
            TimeKeeping = new HashSet<TimeKeeping>();
        }

        public int Id { get; set; }
        public string Rname { get; set; }
        public byte Age { get; set; }
        public string Raddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public int PositionId { get; set; }
        public DateTime? StartWorkedAt { get; set; }
        public decimal? Salary { get; set; }

        public Position Position { get; set; }
        public ICollection<MaterialBill> MaterialBill { get; set; }
        public ICollection<SaleBill> SaleBill { get; set; }
        public ICollection<TimeKeeping> TimeKeeping { get; set; }
    }
}
