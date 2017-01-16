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


        #region Properties
        private string _Title = "Dummy movie";
        public string Title {
            get { return _Title; }
            set
            {
                _Title = value;
            }
        }

        private bool _Seen = false;
        public bool Seen {
            get { return _Seen; }
            set
            {
                _Seen = value;
                OnPropertyChanged("Seen");
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

        private string _DateAddedToList = "N/A";
        public string DateAddedToList
        {
            get { return _DateAddedToList; }
            set
            {
                _DateAddedToList = value;
                OnPropertyChanged("DateAddedToList");
            }
        }

        private string _DateLastRedownload = "N/A";
        public string DateLastRedownload
        {
            get { return _DateLastRedownload; }
            set
            {
                _DateLastRedownload = value;
            }
        }

        private string _MyRating = "N/A";
        public string MyRating
        {
            get { return _MyRating; }
            set
            {
                _MyRating = value;
                OnPropertyChanged("MyRating");
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int MyRatingInt
        {
            get
            {
                if (MyRating == "N/A")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(MyRating);
                }
            }
            set
            {
                MyRating = value.ToString();
            }
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
            set
            {
                if (value == "N/A")
                {
                    return;
                }
                _PosterLink = value;
            }
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

        [Newtonsoft.Json.JsonIgnore]
        public double imdbRatingDouble
        {
            get
            {
                if (imdbRating == "N/A")
                {
                    return 0.0;
                }
                //return Convert.ToDouble(imdbRating);
                return double.Parse(imdbRating, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private string _Metascore = "N/A";
        public string Metascore
        {
            get { return _Metascore; }
            set { _Metascore = value; }
        }

        private string _imdbVotes = "N/A";
        public string imdbVotes
        {
            get { return _imdbVotes; }
            set { _imdbVotes = value; }
        }

        private string _tomateMeter = "N/A";
        public string tomatoMeter
        {
            get { return _tomateMeter; }
            set { _tomateMeter= value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string tomatoMeterForView
        {
            get { return _tomateMeter + " / 100"; }
        }

        private string _tomatoRating = "N/A";
        public string tomatoRating
        {
            get { return _tomatoRating; }
            set { _tomatoRating= value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string tomatoRatingForView
        {
            get { return _tomatoRating + " / 10"; }
        }

        private string _tomatoReviews = "N/A";
        public string tomatoReviews
        {
            get { return _tomatoReviews; }
            set { _tomatoReviews= value; }
        }

        private string _tomatoFresh = "N/A";
        public string tomatoFresh
        {
            get { return _tomatoFresh; }
            set { _tomatoFresh = value; }
        }

        private string _tomatoRotten = "N/A";
        public string tomatoRotten
        {
            get { return _tomatoRotten; }
            set { _tomatoRotten = value; }
        }

        private string _tomatoConsensus = "N/A";
        public string tomatoConsensus
        {
            get { return _tomatoConsensus; }
            set { _tomatoConsensus = value; }
        }

        private string _tomatoUserMeter = "N/A";
        public string tomatoUserMeter
        {
            get { return _tomatoUserMeter; }
            set { _tomatoUserMeter = value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string tomatoUserMeterForView
        {
            get { return _tomatoUserMeter + " / 100"; }
        }

        private string _tomatoUserRating = "N/A";
        public string tomatoUserRating
        {
            get { return _tomatoUserRating; }
            set { _tomatoUserRating= value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string tomatoUserRatingForView
        {
            get { return _tomatoUserRating + " / 5"; }
        }

        private string _tomatoUserReviews = "N/A";
        public string tomatoUserReviews
        {
            get { return _tomatoUserReviews; }
            set { _tomatoUserReviews = value; }
        }

        private string _DVD = "N/A";
        public string DVD
        {
            get { return _DVD; }
            set { _DVD = value; }
        }

        private string _BoxOffice = "N/A";
        public string BoxOffice
        {
            get { return _BoxOffice; }
            set { _BoxOffice = value; }
        }

        private string _Production = "N/A";
        public string Production
        {
            get { return _Production; }
            set { _Production = value; }
        }
        #endregion


        public async Task<string> LoadDataWithID(string ID)
        {
            OmdbApiRequest request = new OmdbApiRequest();
            string result = await request.RequestWithID(ID);
            if (result.StartsWith(";"))
            {
                return result.Substring(1);
            }
            parseData(result);
            return "";
        }

        private void parseData(string jsonData)
        {
            if (jsonData == null)
            {
                return;
            }

            //System.Console.WriteLine("\n\n" + jsonData);
            //System.Console.ReadKey();
            //JObject obj;
            //try
            //{
            //    obj = JObject.Parse(jsonData);
            //}
            //catch (Exception e)
            //{
            //    return;
            //}

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
            this.Metascore = (string)obj["Metascore"];
            this.imdbVotes = (string)obj["imdbVotes"];
            this.tomatoMeter = (string)obj["tomatoMeter"];
            this.tomatoRating = (string)obj["tomatoRating"];
            this.tomatoReviews = (string)obj["tomatoReviews"];
            this.tomatoFresh = (string)obj["tomatoFresh"];
            this.tomatoRotten= (string)obj["tomatoRotten"];
            this.tomatoConsensus = (string)obj["tomatoConsensus"];
            this.tomatoUserMeter = (string)obj["tomatoUserMeter"];
            this.tomatoUserRating = (string)obj["tomatoUserRating"];
            this.tomatoUserReviews = (string)obj["tomatoUserReviews"];
            this.DVD = (string)obj["DVD"];
            this.BoxOffice = (string)obj["BoxOffice"];
            this.Production= (string)obj["Production"];
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
    }
}
