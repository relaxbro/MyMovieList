using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList
{
    public class Movie: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Movie()
        {

        }
        public Movie(string ID)
        {
            this.LoadDataWithID(ID);
        }
        //public Movie(string jsonData)
        //{
        //    this.parseData(jsonData);
        //}

        //private bool _FullData = false;
        //public bool FullData
        //{
        //    get { return _FullData; }
        //}

        private string _Title;
        public string Title {
            get { return _Title; }
            set
            {
                _Title = value;
                //if (MovieChanged != null)
                //{
                //    MovieChanged(this, new ModelChangedEventArgs { OldValue = _Title, NewValue = value});
                //}
            }
        }

        private bool _Seen = false;
        public bool Seen {
            get { return _Seen; }
            set
            {
                _Seen = value;
                OnPropertyChanged("Seen");
                //if (MovieChanged != null)
                //{
                //    MovieChanged(this, new ModelChangedEventArgs { OldValue = _Seen, NewValue = value });
                //}
            }
        }

        private string _FirstSeen = "N/A";
        public string FirstSeen
        {
            get { return _FirstSeen; }
            set
            {
                _FirstSeen = value;
                OnPropertyChanged("FirstSeen");
            }
        }

        private string _LastSeen = "N/A";
        public string LastSeen
        {
            get { return _LastSeen; }
            set
            {
                _LastSeen = value;
                OnPropertyChanged("LastSeen");
            }
        }

        private string _MyRating = "N/A";
        public string MyRating
        {
            get { return _MyRating; }
            set { _MyRating = value; }
        }

        private string _MyComment = "No comments yet.";
        public string MyComment
        {
            get { return _MyComment; }
            set { _MyComment = value; }
        }

        private string _Year = "N/A";
        public string Year
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

        //private string _imdbURL;
        public string imdbURL
        {
            get
            {
                if (imdbID != null)
                {
                    return "http://akas.imdb.com/title/" + imdbID;
                }
                else
                {
                    return "URL N/A";
                }
            }
        }

        private string _Released = "N/A";
        public string Released
        {
            get { return _Released; }
            set { _Released = value; }
        }

        private string _Runtime = "N/A";
        public string Runtime
        {
            get { return _Runtime; }
            set { _Runtime = value; }
        }

        private string _Genre = "N/A";
        public string Genre
        {
            get { return _Genre; }
            set { _Genre = value; }
        }

        private string _Director = "N/A";
        public string Director
        {
            get { return _Director; }
            set { _Director = value; }
        }

        private string _Writer = "N/A";
        public string Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }

        private string _Actors = "N/A";
        public string Actors
        {
            get { return _Actors; }
            set { _Actors = value; }
        }

        private string _Plot = "N/A";
        public string Plot
        {
            get { return _Plot; }
            set { _Plot = value; }
        }

        private string _PosterLink = "/MyMovieList;component/Resources/noposter.jpg";
        public string PosterLink
        {
            get { return _PosterLink; }
            set { _PosterLink = value; }
        }

        private string _Language = "N/A";
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        private string _Country = "N/A";
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Awards = "N/A";
        public string Awards
        {
            get { return _Awards; }
            set { _Awards = value; }
        }

        private string _imdbRating = "N/A";
        public string imdbRating
        {
            get { return _imdbRating; }
            set { _imdbRating = value; }
        }

        public void LoadDataWithID(string ID)
        {
            OmdbApiRequest request = new OmdbApiRequest();
            parseData(request.RequestWithID(ID));
        }

        private void parseData(string jsonData)
        {
            if (jsonData == null)
            {
                return;
            }

            JObject obj = JObject.Parse(jsonData);

            if ((string)obj["Response"] == "False")
            {
                return;
            }

            this.Title = (string)obj["Title"];
            this.Year = (string)obj["Year"];
            this.imdbID = (string)obj["imdbID"];
            this.Released = (string)obj["Released"];
            this.Runtime = (string)obj["Runtime"];
            this.Genre = (string)obj["Genre"];
            this.Director = (string)obj["Director"];
            this.Writer = (string)obj["Writer"];
            this.Actors = (string)obj["Actors"];
            this.Plot = (string)obj["Plot"];
            this.PosterLink = (string)obj["Poster"];
            this.Language = (string)obj["Language"];
            this.Country = (string)obj["Country"];
            this.Awards = (string)obj["Awards"];
            this.imdbRating = (string)obj["imdbRating"];
            //this._FullData = true;

        }

        private void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            var changed = PropertyChanged;
            if (changed != null)
            {
                changed(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public void parsePartialData(string jsonData)
        //{
        //    JObject obj = JObject.Parse(jsonData);

        //    this.Title = (string)obj["Title"];
        //    this.Year = (string)obj["Year"];
        //    this.imdbID = (string)obj["imdbID"];
        //}

        //public delegate void MovieChangedEvent(object sender, ModelChangedEventArgs e);
        //public event MovieChangedEvent MovieChanged;
    }
}
