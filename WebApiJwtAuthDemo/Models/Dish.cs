using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyRestaurant.Models
{
    public partial class Dish
    {
        public Dish()
        {
            SaleBillDetail = new HashSet<SaleBillDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }      
        public decimal Price { get; set; }        
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public byte[] CreatedAt { get; set; }

        public Category Category { get; set; }
        public ICollection<SaleBillDetail> SaleBillDetail { get; set; }
    }
}
