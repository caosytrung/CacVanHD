using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Ruser
    {
        public int Id { get; set; }
        public string Rusername { get; set; }
        public string Rpassword { get; set; }
        public string Emai { get; set; }
        public string Avatar { get; set; }
        public int RoleId { get; set; }
        public byte[] CreateAt { get; set; }

        public Role Role { get; set; }
    }
}
