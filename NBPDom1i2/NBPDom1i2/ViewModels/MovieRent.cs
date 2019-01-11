using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;

namespace NBPDom1i2.ViewModels
{
    public class MovieRent
    {
        public List<string> movietitles { get; set; }
        public List<string> movierentdates { get; set; }
        /*public string movietitle { get; set; }
        public string movierentdate { get; set; }*/ //mozda kao DateTime 

        public MovieRent()
        {
            movietitles = new List<string>();
            movierentdates = new List<string>();
        }
    }
}