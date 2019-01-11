using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;
using NBPDom1i2.ViewModels;

namespace NBPDom1i2.Models
{
    public class CustomersSelectionViewModel
    {
        public List<SelectCustomerEditorViewModel> customers { get; set; }
        
        public CustomersSelectionViewModel()
        {
            customers = new List<SelectCustomerEditorViewModel>();
        }

        public IEnumerable<string> getSelectedUsernames()
        {
            return (from c in this.customers where c.selected select c.username).ToList();
        }
    }
}