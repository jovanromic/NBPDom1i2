using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using System.Configuration;
using NBPDom1i2.ViewModels;
using System.Text;

namespace NBPDom1i2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {   
            //
            var host = ConfigurationManager.AppSettings["host"].ToString();
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            RedisEndpoint _redisEndpoint = new RedisEndpoint(host, port);
            //

            HomeList homelist = new HomeList();

            if ((string)Session["role"] == "customer")
            {
                byte[][] history;
                using (var redisClient = new RedisClient(_redisEndpoint))
                {
                    //byte[] array = Encoding.Default.GetBytes((string)Session["username"]);
                    history = redisClient.LRange((string)Session["username"], 0, 9);
                    foreach (byte[] arr in history )
                    {
                        homelist.historytitles.Add(Encoding.Default.GetString(arr));
                    }
                }
            }

            if ((string)Session["role"] == "admin")
            {
                Dictionary<string, string> tempdict = new Dictionary<string, string>();
                byte[][] rented;
                using (var redisClient = new RedisClient(_redisEndpoint))
                {

                    rented = redisClient.HGetAll("rented");
                }

                for (int i = rented.Length -1; i > 0; i -= 2)
                {
                    string value = Encoding.Default.GetString(rented[i]);
                    string key = Encoding.Default.GetString(rented[i-1]);
                    tempdict.Add(key, value);
                }
                int gr = 10;
                if (tempdict.Count < 10)
                {
                    gr = tempdict.Count;
                }
                int ind = 0;
                foreach (var v in tempdict)
                {
                    homelist.rentedlist.Add(v.Key, v.Value);
                    ind++;
                    if (ind == gr)
                    {
                        break;
                    }
                }
            }

            Dictionary<string, int> dict = new Dictionary<string, int>();
            byte[][] ar;

            using (var redisClient = new RedisClient(_redisEndpoint))
            {          
                ar = redisClient.HGetAll("movies");
            }
            for (int i = 0; i<ar.Length; i+=2)
            {
                string key = Encoding.Default.GetString(ar[i]);
                int value = int.Parse(Encoding.Default.GetString(ar[i + 1]));
                dict.Add(key, value);
            }

            dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            int granica = 15;

            if (dict.Count < 15)
            {
                granica = dict.Count;
                
            }
            int index = 0;
            foreach (var v in dict)
            {
                homelist.top15.Add(v.Key, v.Value);
                index++;
                if (index == granica)
                {
                    break;
                }
            }
            return View(homelist);
        }

        /*public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }*/
    }
}