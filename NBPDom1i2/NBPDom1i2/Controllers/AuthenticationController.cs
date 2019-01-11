using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPDom1i2.Models;

namespace NBPDom1i2.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            if (Session["username"] != null)
            { 

                return RedirectToAction("Index", "Movies");
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult LogIn(Customer customer)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("username", customer.username);
            dictionary.Add("password", customer.password);

            var data =  WebApiConfig.GraphClient.Cypher.Match
                ("(c:Customer {username:{username}, password:{password}})")
                .WithParams(dictionary)
                .Return(c => c.As<Customer>())
                .Results;

            List<Customer> customers = data.ToList();

            if (customers.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Session["username"] = customers[0].username;
                Session["role"] = customers[0].role;

                return RedirectToAction("Index", "Home");
            }
           
        }

        public ActionResult LogOut()
        {
            Session["username"] = null;
            Session["role"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}