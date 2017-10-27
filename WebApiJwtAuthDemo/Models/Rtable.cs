using System;
using System.Collections.Generic;

namespace MyRestaurant.Models
{
    public partial class Rtable
    {
        public Rtable()
        {
            BookTable = new HashSet<BookTable>();
        }

        public int Id { get; set; }
        public byte NumberOfSeat { get; set; }
        public byte Available { get; set; }
        public string TypeTable { get; set; }
        public string LocationTable { get; set; }
        public string Thumbnail { get; set; }

        public ICollection<BookTable> BookTable { get; set; }
    }
}
