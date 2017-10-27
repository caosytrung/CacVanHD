using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Position
    {
        public Position()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string WorkDesc { get; set; }
        public byte? Deleted { get; set; }
        public string Name { get; set; }
        public byte[] CreatedAt { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
