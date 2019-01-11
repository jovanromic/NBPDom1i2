using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;
using NBPDom1i2.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace NBPDom1i2.ViewModels
{
    public class TitleGenreActorDirectorView
    {
        //[Display(Name = "Search by title")]
        public string title { get; set; }

        //
        //public List<string> movietitles { get; set; }
        //public List<string> moviegenres { get; set; }

        //public Dictionary<string, string> moviegenre { get; set; }
        //public MovieGenre moviegenre { get; set; }
        public List<MovieGenre> moviegenre { get; set; }
        //

        [Display(Name = "Genre")]
        public List<string> genres { get; set; }
        [Display(Name = "Actor")]
        public List<string> actors { get; set; }
        [Display(Name = "Director")]
        public List<string> directors { get; set; }

        public TitleGenreActorDirectorView()
        {
            //movietitles = new List<string>();
            //moviegenres = new List<string>();
            //moviegenre = new Dictionary<string, string>();
            moviegenre = new List<MovieGenre>();
            genres = new List<string>();
            actors = new List<string>();
            directors = new List<string>();
        }
    }
    
}