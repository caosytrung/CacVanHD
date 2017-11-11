using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int RoleId { get; set; }
        public byte[] CreatedAt { get; set; }

        public Role Role { get; set; }
    }
}
