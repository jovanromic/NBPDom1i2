using NBPDom1i2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBPDom1i2.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewCustomer()
        {
            Customer c = new Customer { name = "Chris", surname = "Jones" };
            return View(c);
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", customer.name);
            dictionary.Add("surname", customer.surname);
            WebApiConfig.GraphClient.Cypher.Create("(customer:Customer {name: {name}, surname: {surname}})")
                .WithParams(dictionary).ExecuteWithoutResults();

            return RedirectToAction("Index", "Home");
        }
    }
}