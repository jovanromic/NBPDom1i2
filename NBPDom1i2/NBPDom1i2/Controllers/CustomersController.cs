using NBPDom1i2.Models;
using NBPDom1i2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jClient;
using Neo4jClient.Cypher;

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

            //var model = new CustomersSelectionViewModel();

            List<SelectCustomerEditorViewModel> model = new List<SelectCustomerEditorViewModel>();

            foreach (var customer in data)
            {
                var editorViewModel = new SelectCustomerEditorViewModel()
                {
                    name = string.Format("{0} {1}", customer.name, customer.surname),
                    username = customer.username,
                    selected = false
                };
                model.Add(editorViewModel);
            }

            return View(model);
        }

        public ActionResult NewCustomer()
        {
            Customer c = new Customer { name = "", surname = "" };
            return View(c);
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", customer.name);
            dictionary.Add("surname", customer.surname);
            dictionary.Add("username", customer.username);
            dictionary.Add("password", customer.password);
            dictionary.Add("role", "customer");
            //WebApiConfig.GraphClient.Cypher.Create("(customer:Customer {name: {name}, surname: {surname}, username: {username}, password: {password}, role: {customer}})")
            //    .WithParams(dictionary).ExecuteWithoutResults();
            try
            {
                var data = WebApiConfig.GraphClient.Cypher.Create("(c:Customer {name: {name}, surname: {surname}, username: {username}, password: {password}, role: {role}})")
                    .WithParams(dictionary)
                    .Return(c => c.As<Customer>())
                    .Results;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                Session["failedRegister"] = "failed";
                customer.username = "";
                //Customer c = new Customer
                //{ name = "", surname = "", username = "", password = "" };
                return View("NewCustomer",customer);
            }

            
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string Username)
        {
            /*Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary*/
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("username", Username);
                /*WebApiConfig.GraphClient.Cypher.Delete("(customer:Customer {username: {username}})")
                .WithParams(dictionary).ExecuteWithoutResults();*/

            var query = new Neo4jClient.Cypher.CypherQuery(
               "match (customer:Customer {username: {username}}) detach delete customer"
               , dictionary, Neo4jClient.Cypher.CypherResultMode.Set);

            ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteCypher(query);

            //return RedirectToAction("Index", "Customers");
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult MyRents()
        {
            if(Session["username"]!=null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("username", Session["username"]);
                MyRentMovie rd = new MyRentMovie();


                var data = WebApiConfig.GraphClient.Cypher
                    .Match("(m:Movie)<-[r:RENTS]-(:Customer {username:{username}})")
                    .WithParams(dictionary)
                    .Return((m, r) => new
                    {
                        Movie = m.As<Movie>(),
                        Rentdate = r.As<MyRentMovie>()
                    });

                var results = data.Results.ToList();

                //MovieRent movierents = new MovieRent();

                List<MyRentMovie> mrmovies = new List<MyRentMovie>();

                foreach (var result in results)
                {
                    result.Rentdate.rentedtitle = result.Movie.title;
                    mrmovies.Add(result.Rentdate);
                    /*movierents.movietitles.Add(result.Movie.title);
                    movierents.movierentedondates.Add(result.Rentdate.rentedon);
                    movierents.movieexpirydates.Add(result.Rentdate.expiry);
                    movierents.moviereturnedondates.Add(result.Rentdate.returnedon);*/
                }

                return View(mrmovies);
            }
            else
                return RedirectToAction("LogIn", "Authentication");
        }
    }
}