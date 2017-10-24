using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Role
    {
        public Role()
        {
            Ruser = new HashSet<Ruser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ruser> Ruser { get; set; }
    }
}
