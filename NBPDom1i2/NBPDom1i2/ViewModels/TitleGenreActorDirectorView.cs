using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPDom1i2.Models;

namespace NBPDom1i2.ViewModels
{
    public class TitleGenreActorDirectorView
    {
        public string title { get; set; }

        public List<string> genres { get; set; }
        public List<string> actors { get; set; }
        public List<string> directors { get; set; }

        public TitleGenreActorDirectorView()
        {
            genres = new List<string>();
            actors = new List<string>();
            directors = new List<string>();
        }
    }
    
}