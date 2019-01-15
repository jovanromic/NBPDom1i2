using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPDom1i2.ViewModels
{
        public class HomeList
        {
            public List<string> historytitles { get; set; }
            public Dictionary<string, int> top15 { get; set; }
            public Dictionary<string, string> rentedlist { get; set; }

            public HomeList()
            {
                historytitles = new List<string>();
                top15 = new Dictionary<string, int>();
                rentedlist = new Dictionary<string, string>();
            }
        }
}