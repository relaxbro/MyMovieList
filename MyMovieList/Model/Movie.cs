using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyMovieList
{
    class Movie
    {
        public Movie()
        {

        }
        public Movie(string jsonData)
        {
            this.parseData(jsonData);
        }

        private string _Title;
        public string Title {
            get { return _Title; }
            set { _Title = value;}
        }

        private bool _Seen = false;
        public bool Seen {
            get { return _Seen; }
            set { _Seen = value; }
        }

        private int _Year;
        public int Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _imdbID;
        public string imdbID
        {
            get { return _imdbID; }
            set { _imdbID = value; }
        }

        private string _Released;
        public string Released
        {
            get { return _Released; }
            set { _Released = value; }
        }

        private string _Runtime;
        public string Runtime
        {
            get { return _Runtime; }
            set { _Runtime = value; }
        }

        private string _Director;
        public string Director
        {
            get { return _Director; }
            set { _Director = value; }
        }

        private string _Writer;
        public string Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }

        private string _Actors;
        public string Actors
        {
            get { return _Actors; }
            set { _Actors = value; }
        }

        private string _Plot;
        public string Plot
        {
            get { return _Plot; }
            set { _Plot = value; }
        }

        private string _PosterLink;
        public string PosterLink
        {
            get { return _PosterLink; }
            set { _PosterLink = value; }
        }

        private string _Language;
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Awards;
        public string Awards
        {
            get { return _Awards; }
            set { _Awards = value; }
        }

        private string _imdbRating;
        public string imdbRating
        {
            get { return _imdbRating; }
            set { _imdbRating = value; }
        }

        private void parseData(string jsonData)
        {
            JObject obj = JObject.Parse(jsonData);

            if ((string)obj["Response"] == "False")
            {
                return;
            }

            this.Title = (string)obj["Title"];
            this.Year = (int)obj["Year"];
            this.imdbID = (string)obj["imdbID"];
            this.Released = (string)obj["Released"];
            this.Runtime = (string)obj["Runtime"];
            this.Director = (string)obj["Director"];
            this.Writer = (string)obj["Writer"];
            this.Actors = (string)obj["Actors"];
            this.Plot = (string)obj["Plot"];
            this.PosterLink = (string)obj["Poster"];
            this.Language = (string)obj["Language"];
            this.Country = (string)obj["Country"];
            this.Awards = (string)obj["Awards"];
            this.imdbRating = (string)obj["imdbRating"];
        }
    }
}
