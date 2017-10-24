using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class MaterialBill
    {
        public MaterialBill()
        {
            MaterialBillDetail = new HashSet<MaterialBillDetail>();
        }

        public int Id { get; set; }
        public int EmployeeRespond { get; set; }
        public DateTime? CreateAt { get; set; }
        public decimal? Total { get; set; }
        public string Rtype { get; set; }

        public Employee EmployeeRespondNavigation { get; set; }
        public ICollection<MaterialBillDetail> MaterialBillDetail { get; set; }
    }
}
