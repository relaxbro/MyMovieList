using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace MyMovieList.Model
{
    [DataContract]
    public class OmdbSearchResult
    {
        public OmdbSearchResult() { }

        public OmdbSearchResult(string title, string year, string imdbID)
        {
            _Title = title;
            _Year = year;
            _imdbID = imdbID;
        }

        private string _Title = "";
        [DataMember]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Year = "";
        [DataMember]
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _imdbID = "";
        [DataMember]
        public string imdbID
        {
            get { return _imdbID; }
            set { _imdbID = value; }
        }
    }
}
