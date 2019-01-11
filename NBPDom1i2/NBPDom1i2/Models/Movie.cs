using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPDom1i2.Models
{
    public class Movie
    {
        //public int id { get; set; }
        public string title { get; set; }
        public int released { get; set; }
        public string description { get; set; }
        public int copies { get; set; }
    }
}