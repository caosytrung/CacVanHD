using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Category
    {
        public Category()
        {
            Dish = new HashSet<Dish>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] CreatedAt { get; set; }

        public ICollection<Dish> Dish { get; set; }
    }
}
