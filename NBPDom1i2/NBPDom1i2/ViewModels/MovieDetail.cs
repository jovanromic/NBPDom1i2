using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;

namespace NBPDom1i2.ViewModels
{
    public class MovieDetail
    {
        public Movie movie { get; set; }
        public List<string> actors { get; set; }
        public string genre { get; set; }
        public string director { get; set; }

        public MovieDetail()
        {
            actors = new List<string>();
        }
    }
}