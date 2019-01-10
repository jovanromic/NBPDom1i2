using NBPDom1i2.Models;
using NBPDom1i2.ViewModels;
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
            //Customers customers = new Customers();

            var data = WebApiConfig.GraphClient.Cypher
                .Match("(c:Customer)")
                .Return(c => c.As<Customer>())
                .Results.ToList();

            var model = new CustomersSelectionViewModel();

            foreach (var customer in data)
            {
                var editorViewModel = new SelectCustomerEditorViewModel()
                {
                    name = string.Format("{0} {1}", customer.name, customer.surname),
                    username = customer.username,
                    selected = false
                };
                model.customers.Add(editorViewModel);
            }

            return View(model);
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
            dictionary.Add("username", customer.username);
            WebApiConfig.GraphClient.Cypher.Create("(customer:Customer {name: {name}, surname: {surname}, username: {username}})")
                .WithParams(dictionary).ExecuteWithoutResults();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DeleteCustomer(CustomersSelectionViewModel customers)
        {
            /*Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary*/
            var selectedUsernames = customers.getSelectedUsernames(); //ovde imam sve usernamee za brisanje
            foreach (var su in selectedUsernames)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("username", su);
                WebApiConfig.GraphClient.Cypher.Delete("(customer:Customer {username: {username}})")
                .WithParams(dictionary).ExecuteWithoutResults();

                //return RedirectToAction("Index", "Customers");
            }
            return RedirectToAction("Index", "Customers");
        }
    }
}