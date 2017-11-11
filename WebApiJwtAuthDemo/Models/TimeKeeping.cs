using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class TimeKeeping
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? WorkHour { get; set; }

        public Employee Employee { get; set; }
    }
}
