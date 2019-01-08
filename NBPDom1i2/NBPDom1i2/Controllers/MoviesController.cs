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

        public ActionResult Index()
        {
            TitleGenreActorDirectorView probni = new TitleGenreActorDirectorView();
            probni.title = "Proba";
            probni.genres.Add("Thriller");
            probni.genres.Add("Action");
            probni.genres.Add("Comedy");
            return View(probni);
        }

        // movies -> za stranu koja ce nam prikazivati listu svih filmova
        /*public ActionResult Index(int? pageIndex, string sortBy) // ? - nullable, 
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortby={1}", pageIndex, sortBy));
        }*/

        [Route("movies/released/{year}/{month:range(1,12)}")] //:range constraint na nivou parametra
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        public ActionResult GetMovies(string title,string genre,string director,string actor)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (title != string.Empty)
            {
                var data = WebApiConfig.GraphClient.Cypher
              .Match("(movie:Movie {title:{title}})")
              .WithParam("title", title)
              .Return(movie => movie.As<Movie>())
              .Results;

                return Content(data.First().title);
            }
            else if (genre != string.Empty && director != string.Empty && actor != string.Empty)
            {
                dictionary.Add("genre", genre);
                dictionary.Add("director", director);
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(: Genre { name: { genre} })," +
                    "(movie) <-[:DIRECTED]-(: Director {name: {director} })," +
                    "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })" +
                    "return movie",
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
            else if (genre != string.Empty && director != string.Empty)
            {
                dictionary.Add("genre", genre);
                dictionary.Add("director", director);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(: Genre { name: { genre} })," +
                    "(movie) <-[:DIRECTED]-(: Director {name: {director} })" +
                    "return movie",
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
            else if (genre != string.Empty && actor != string.Empty)
            {
                dictionary.Add("genre", genre);
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(: Genre { name: { genre} })," +
                    "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })" +
                    "return movie",
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
            else if (director != string.Empty && actor != string.Empty)
            {
                dictionary.Add("director", director);
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) <-[:DIRECTED]-(: Director { name: { director} })," +
                    "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })" +
                    "return movie",
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
            else if (genre != string.Empty)
            {
                dictionary.Add("genre", genre);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(: Genre { name: { genre} })" +
                    "return movie",
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
            else if (director != string.Empty)
            {
                dictionary.Add("director", director);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) <-[:DIRECTED]-(: Director { name: { director} })" +
                    "return movie",
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
            else 
            {
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) <-[:ACTED_IN]-(: Actor { name: { actor} })" +
                    "return movie",
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