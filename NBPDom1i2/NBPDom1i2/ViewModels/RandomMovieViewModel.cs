using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;

namespace NBPDom1i2.ViewModels
{
    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }
    }  
}