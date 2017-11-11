using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestaurant.Options
{
    public class Response
    {
        public int code { get; set; }
        public string message { get; set; }
        public Object data { get; set; }
    }
}
