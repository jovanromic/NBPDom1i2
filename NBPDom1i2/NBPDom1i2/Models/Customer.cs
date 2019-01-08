using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NBPDom1i2.Models
{
    public class Customer
    {
        //public int Id { get; set; }
        [Display(Name ="Name")]
        public string name { get; set; }

        [Display(Name ="Surname")]
        public string surname { get; set; }

    }
}