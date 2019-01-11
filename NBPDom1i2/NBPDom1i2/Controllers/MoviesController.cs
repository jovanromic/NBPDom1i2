﻿using System;
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

        //[HttpGet]
        public ActionResult Index()
        {
            //ovde upiti ka bazi 
            // +viewmodel za tabelu MovieGenre


            TitleGenreActorDirectorView probni = new TitleGenreActorDirectorView();
            probni.title = "";

            var data = WebApiConfig.GraphClient.Cypher
                .Match("(g:Genre)")
                .Return(g => g.As<Genre>())
                .Results;

            var data1 = WebApiConfig.GraphClient.Cypher
                .Match("(d:Director)")
                .Return(d => d.As<Director>())
                .Results;

            var data2 = WebApiConfig.GraphClient.Cypher
                .Match("(a:Actor)")
                .Return(a => a.As<Actor>())
                .Results;

            var data3 = WebApiConfig.GraphClient.Cypher
                .Match("(movie:Movie)-[:OF_TYPE]->(genre:Genre)")
                .Return(() => new
                {
                    Title = Return.As<string>("movie.title"),
                    Genre = Return.As<string>("genre.name")
                })
                .Results;

               //var res = q.Results;


            probni.genres.Add("");
            probni.directors.Add("");
            probni.actors.Add("");

            foreach (Genre g in data)
            {
                probni.genres.Add(g.name);
            }

            
            foreach (Director d in data1)
            {
                probni.directors.Add(d.name);
            }

           
            foreach (Actor a in data2)
            {
                probni.actors.Add(a.name);
            }

            foreach (var m in data3)
            {
                //probni.moviegenre.Add(m.Title, m.Genre);
                probni.moviegenre.Add (new MovieGenre { movietitle = m.Title, moviegenre = m.Genre });
                //probni.moviegenre[i].moviegenre = m.Genre;
                /*probni.movietitles.Add(m.Title);
                probni.moviegenres.Add(m.Genre);*/
            }

            //probni.genres.Add("Thriller");

            return View(probni);
        }

        /*[HttpPost]
        [ActionName("IndexGetMovies")]
        public ActionResult Index(TitleGenreActorDirectorView tgadv)
        {

        }*/

        //public 

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

        [Route("movie/{title}")]
        public ActionResult MovieDetails(string title)
        {
            MovieDetail moviedetail = new MovieDetail();
            //moviedetail.movie = new Movie { title = "Avatar",
            //    description = "Plavi veliki ljudi protiv beli mali ljudi. Biju se.",
            //    released = 2009, copies = 5 };
            //moviedetail.genre = "Thriller";
            //moviedetail.actors = new List<string> { "Actor 1", "Actor 2", "Actor 3" };

            var data = WebApiConfig.GraphClient.Cypher.Match
                ("(movie:Movie {title: {title}})-[OF_TYPE]-(genre:Genre)," +
                //
"(movie)-[ACTED_IN]-(actor:Actor)," +
                "(movie)-[DIRECTED]-(director:Director)")
                .WithParam("title", title)
                .Return(() => new MovieDetail
                {
                    movie = Return.As<Movie>("movie"),
                    genre = Return.As<string>("genre.name"),
                    //actors = Return.As<List<string>>("actor.name"),
                    director = Return.As<string>("director.name")
                }).Results;

            moviedetail = data.ToList()[0];

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("title", title);
            var query = new Neo4jClient.Cypher.CypherQuery(
                "match (actor:Actor)-[ACTED_IN]->(movie:Movie {title: {title}}) return actor.name"
                , dictionary, CypherResultMode.Set);

            moviedetail.actors = ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteGetCypherResults<string>(query).ToList();

            return View("Detail",moviedetail);
        }

        [HttpPost]
        public ActionResult GetMovies(TitleGenreActorDirectorView tgadv)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if(tgadv.title == null)
            {
                tgadv.title = string.Empty;
            }

            string title = tgadv.title;
            //string genre = ;
            string genre = tgadv.genres[0];
            string director = tgadv.directors[0];
            string actor = tgadv.actors[0];

            List<MovieGenre> data;


            //
            var data1 = WebApiConfig.GraphClient.Cypher
                .Match("(g:Genre)")
                .Return(g => g.As<Genre>())
                .Results;

            var data2 = WebApiConfig.GraphClient.Cypher
                .Match("(d:Director)")
                .Return(d => d.As<Director>())
                .Results;

            var data3 = WebApiConfig.GraphClient.Cypher
                .Match("(a:Actor)")
                .Return(a => a.As<Actor>())
                .Results;

            tgadv.genres[0]="";
            tgadv.directors[0]="";
            tgadv.actors[0]="";

            foreach (Genre g in data1)
            {
                tgadv.genres.Add(g.name);
            }


            foreach (Director d in data2)
            {
                tgadv.directors.Add(d.name);
            }


            foreach (Actor a in data3)
            {
                tgadv.actors.Add(a.name);
            }


            //
            if (title != string.Empty)
            {
                data = WebApiConfig.GraphClient.Cypher
                .Match("(movie:Movie {title:{title}})-[:OF_TYPE]->(genre:Genre)")
                .WithParam("title", title)
                .Return(() => new MovieGenre
                {
                    movietitle = Return.As<string>("movie.title"),
                    moviegenre = Return.As<string>("genre.name")
                })
                .Results.ToList<MovieGenre>();

                foreach (var m in data)
                {
                    tgadv.moviegenre.Add(m);
                }
                return View("Index",tgadv);
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
                foreach (Movie m in movies)
                {
                    MovieGenre mg = new MovieGenre { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(mg);
                }
                return View("Index", tgadv);
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

                foreach (Movie m in movies)
                {
                    MovieGenre mg = new MovieGenre() { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(mg);
                }
                return View("Index", tgadv);
            }
            else if (genre != string.Empty && actor != string.Empty)
            {
                dictionary.Add("genre", genre);
                dictionary.Add("actor", actor);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(:Genre { name: {genre} })," +
                    "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })" +
                    "return movie",
                    dictionary, CypherResultMode.Set);

                List<Movie> movies = ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteGetCypherResults<Movie>(query).ToList();

                foreach (Movie m in movies)
                {
                    MovieGenre mg = new MovieGenre { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(mg);
                }
                return View("Index", tgadv);
            }
            else if (director != string.Empty && actor != string.Empty)
            {
                dictionary.Add("director", director);
                dictionary.Add("actor", actor);


                /*var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) <-[:DIRECTED]-(: Director { name: { director} })," +
                    "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })," +
                    "(movie) -[OF_TYPE]-> (g:Genre)" +
                    "return movie.title as movietitle,g.name as moviegenre",
                    dictionary, CypherResultMode.Set);

                List<MovieGenre> moviesgenres = ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteGetCypherResults<MovieGenre>(query).ToList();*/

                data = WebApiConfig.GraphClient.Cypher
                .Match("(movie:Movie)<-[:DIRECTED]-(: Director { name: { director} })," +
                "(movie) <-[:ACTED_IN]-(: Actor {name: {actor} })," +
                "(movie) -[OF_TYPE]-> (g:Genre)")
                .WithParams(dictionary)
                .Return(() => new MovieGenre
                {
                    movietitle = Return.As<string>("movie.title"),
                    moviegenre = Return.As<string>("g.name")
                })
                .Results.ToList<MovieGenre>();

                foreach (MovieGenre m in data)
                {
                    //content += m.title + " " + m.description + "<hr>";
                    //MovieGenre mg = new MovieGenre() { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(m);

                }
                return View("Index", tgadv);
            }
            else if (genre != string.Empty)
            {
                dictionary.Add("genre", genre);
                var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie) -[:OF_TYPE]->(:Genre { name: {genre} })" +
                    "return movie",
                    dictionary, CypherResultMode.Set);

                List<Movie> movies = ((IRawGraphClient)WebApiConfig.GraphClient)
                    .ExecuteGetCypherResults<Movie>(query).ToList();

                foreach (Movie m in movies)
                {
                    MovieGenre mg = new MovieGenre { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(mg);
                }
                return View("Index", tgadv);
            }
            else if (director != string.Empty)
            {
                dictionary.Add("director", director);
                data = WebApiConfig.GraphClient.Cypher
                .Match("(movie:Movie)<-[:DIRECTED]-(:Director { name: {director} })," +
                "(movie) -[OF_TYPE]-> (g:Genre)")
                .WithParams(dictionary)
                .Return(() => new MovieGenre
                {
                    movietitle = Return.As<string>("movie.title"),
                    moviegenre = Return.As<string>("g.name")
                })
                .Results.ToList<MovieGenre>();

                foreach (MovieGenre m in data)
                {
                    //content += m.title + " " + m.description + "<hr>";
                    //MovieGenre mg = new MovieGenre() { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(m);

                }
                return View("Index", tgadv);
            }
            else if (actor != string.Empty)
            {
                dictionary.Add("actor", actor);
                data = WebApiConfig.GraphClient.Cypher
                .Match("(movie:Movie)<-[:ACTED_IN]-(: Actor { name: { actor} })," +
                "(movie) -[OF_TYPE]-> (g:Genre)")
                .WithParams(dictionary)
                .Return(() => new MovieGenre
                {
                    movietitle = Return.As<string>("movie.title"),
                    moviegenre = Return.As<string>("g.name")
                })
                .Results.ToList<MovieGenre>();

                foreach (MovieGenre m in data)
                {
                    //content += m.title + " " + m.description + "<hr>";
                    //MovieGenre mg = new MovieGenre() { movietitle = m.title, moviegenre = genre };
                    tgadv.moviegenre.Add(m);

                }
                return View("Index", tgadv);
            }
            else 
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Rent(MovieDetail moviedetail)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("title", moviedetail.movie.title);
            dictionary.Add("username", (string)Session["username"]);
            dictionary.Add("copies", moviedetail.movie.copies - 1);

            DateTime expdate = DateTime.Now;
            //expdate.AddMonths(1);
            dictionary.Add("expiry", expdate.ToString("yyyy-MM-dd"));

            //WebApiConfig.GraphClient.Cypher.Match
            //    ("(movie:Movie {title: {title}}),(customer:Customer {username: {username}})")
            //    .WithParams(dictionary)
            //    .Set("(movie {copies: {copies}})")
            //    .Create("(customer)-[r:RENTS {expiry: {expiry}}]->(movie)")
            //    .ExecuteWithoutResults;

            var query = new Neo4jClient.Cypher.CypherQuery("match (movie:Movie {title:{title}})," +
                "(customer:Customer {username: {username}})" +
                "set movie.copies = {copies}" +
                "create (customer)-[r:RENTS {expiry:{expiry}}]->(movie)" +
                "return movie",
                dictionary, CypherResultMode.Set);

            List<Movie> movies = ((IRawGraphClient)WebApiConfig.GraphClient)
                .ExecuteGetCypherResults<Movie>(query).ToList();

            return Content("Iznajmljeno");

        }
    }
}