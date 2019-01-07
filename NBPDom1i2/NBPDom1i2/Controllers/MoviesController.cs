using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPDom1i2.Models;
using NBPDom1i2.ViewModels;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace NBPDom1i2.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random
        public ActionResult Random()
        {
            //var movie = new Movie() { Name = "Godfather" };
            var customers = new List<Customer>
            {
                new Customer { name = "Customer1" },
                new Customer { name = "Customer2" }
            };


            //string title = "Ocean's Eleven";

            //var data = WebApiConfig.GraphClient.Cypher
            //    .Match("(movie:Movie)")
            //    .Return(movie => movie.As<Movie>())
            //    .Results;

            string title = "Ocean's Eleven";

            var data = WebApiConfig.GraphClient.Cypher
            .Match("(movie:Movie {title:{title}})")
            .WithParam("title", title)
            .Return(movie => movie.As<Movie>())
            .Results;

            //Movie m = WebApiConfig.GraphClient.Get<Movie>(1).Data;
            var viewModel = new RandomMovieViewModel
            {
                Movie = data.Single(),
                Customers = customers
            };

            return View(viewModel); 
        }

        public ActionResult Edit(int id)
        {
            return Content("Id=" + id);
        }

        // movies -> za stranu koja ce nam prikazivati listu svih filmova
        public ActionResult Index(int? pageIndex, string sortBy) // ? - nullable, 
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortby={1}", pageIndex, sortBy));
        }

        [Route("movies/released/{year}/{month:range(1,12)}")] //:range constraint na nivou parametra
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        public ActionResult GetMovies(string title,string genre,string director,string actor)
        {
            if (title != string.Empty)
            {
                var data = WebApiConfig.GraphClient.Cypher
              .Match("(movie:Movie {title:{title}})")
              .WithParam("title", title)
              .Return(movie => movie.As<Movie>())
              .Results;

                return Content(data.First().title);
            }
            else
            {
                //var data = WebApiConfig.GraphClient.Cypher
                //    .Match("(movie:Movie)")
                //    .Where("{genre} IS NULL OR genre:{genre}")
                //    .WithParam("genre",genre)
                //    .Return(movie => movie.As<Movie>())
                //    .Results;

                //string content ="" ;
                //foreach(Movie m in data)
                //{
                //    content += m.title + " " + m.description + "\n";
                //}
                //return Content(content);
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary["genre"] = genre;
                //if (genre == string.Empty)
                //    genre = null;
                dictionary.Add("genre", genre);
                dictionary.Add("director", director);
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie1:Movie) where " +
                    "{ genre } is null or(movie1) -[:OF_TYPE]->(: Genre { name: { genre} })" +
                //    "match (movie2:Movie) where" +
                //    " {director} is null or (movie1) -[:DIRECTED]-(:Director { name: {director} })" +
                //    "match (movie3:Movie) where" +
                //    " {actor} is null or (movie2) -[:ACTED_IN]-(:Actor { name: {actor} })  " +

                    "return distinct movie1",
                    dictionary, CypherResultMode.Set);


                List<Movie> movies = ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteGetCypherResults<Movie>(query).ToList();

                string content = "";
                foreach (Movie m in movies)
                {
                    content += m.title + " " + m.description + "<hr>";
                }
                return Content(content);
            }

            
        }
    }
}