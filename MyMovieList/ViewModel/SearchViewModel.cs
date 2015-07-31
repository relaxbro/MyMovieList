using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        ObservableCollection<OmdbSearchResult> _OmdbSearchResults = new ObservableCollection<OmdbSearchResult>();

        public SearchViewModel()
        {
            System.Console.WriteLine("creating new searchviewmodel");
        }

        private string _NewSearchTerm = "";
        public string NewSearchTerm
        {
            get
            {
                return _NewSearchTerm;
            }
            set
            {
                _NewSearchTerm = value;
                onPropertyChanged("NewSearchTerm");
                //SearchStatus = "Searching for " + _NewSearchTerm;
            }
        }

        private string _SearchStatus;
        public string SearchStatus
        {
            get
            {
                return _SearchStatus;
            }
            set
            {
                _SearchStatus = value;
                onPropertyChanged("SearchStatus");
            }
        }

        public ObservableCollection<OmdbSearchResult> OmdbSearchResults
        {
            get { return _OmdbSearchResults; }
            set
            {
                _OmdbSearchResults = value;
                onPropertyChanged("OmdbSearchResults");
            }
        }

        private OmdbSearchResult _SelectedResult;
        public OmdbSearchResult SelectedResult
        {
            get { return _SelectedResult; }
            set
            {
                _SelectedResult = value;
                onPropertyChanged("SearchResult");
            }
        }

        public string UseSearchParser(string search)
        {
            SearchParser searchParser = new SearchParser();
            return searchParser.Search(search);
        }

        #region Search
        public ICommand NewSearch
        {
            get
            {
                return new RelayCommand(ExecuteNewSearch, CanNewSearch);
            }
        }

        public void ExecuteNewSearch(object parameter)
        {

            _OmdbSearchResults.Clear();
            System.Console.WriteLine("inside ExecuteNewSearch");
            System.Console.WriteLine("NewSearchTerm: " + NewSearchTerm);

            SearchStatus = "Searching for " + NewSearchTerm;
            string searchResult = UseSearchParser(NewSearchTerm);
            System.Console.WriteLine(searchResult);


            string[] searchResultSplit = searchResult.Split(':');
            if (searchResultSplit[0] == "{\"Title\"")
            {
                System.Console.WriteLine("single result");
                JObject obj = JObject.Parse(searchResult);
                _OmdbSearchResults.Add(new OmdbSearchResult((string)obj["Title"], (string)obj["Year"], (string)obj["imdbID"]));
                SearchStatus = "Found 1 movie.";

            }
            else if (searchResultSplit[0] == "{\"Search\"")
            {
                string searchResultTrimmed = "[" + searchResult.Split('[', ']')[1] +"]";

                System.Console.WriteLine("multiple results");
                JSonHelper jsonHelper = new JSonHelper();
                OmdbSearchResults = jsonHelper.ConvertJSonToObject<ObservableCollection<OmdbSearchResult>>(searchResultTrimmed);
                SearchStatus = "Found " + _OmdbSearchResults.Count + " movies.";

                //System.Runtime.Serialization.Json.DataContractJsonSerializer deserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(OmdbSearchResult));
                //MemoryStream memStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(searchResult));
                //memStream.Position = 0;
                //_OmdbSearchResults.Add((OmdbSearchResult)deserializer.ReadObject(memStream));
                //using (MemoryStream memStream = new MemoryStream(Encoding.Unicode.GetBytes(searchResult)))
                //{
                    //OmdbSearchResult result = (OmdbSearchResult)deserializer.ReadObject(memStream);
                    //OmdbSearchResults = deserializer.ReadObject(memStream) as ObservableCollection<OmdbSearchResult>;
                    //Console.WriteLine("yeah trying to deserialize " + result.Title);
                //}
                if (_OmdbSearchResults == null)
                {
                    Console.WriteLine("Fuck observablecollection is null");
                }

                //int counter = 0;
                //foreach (OmdbSearchResult omdbres in _OmdbSearchResults)
                //{
                //    counter++;
                //    System.Console.WriteLine(counter);
                //    System.Console.WriteLine(omdbres.Title);
                //}
            }
            else
            {
                Console.WriteLine("no valid results");
                SearchStatus = "No results found: " + searchResult;
            }

        }

        public bool CanNewSearch(object parameter)
        {
            return true;
        }
        #endregion


        #region Select
        public ICommand SelectResult
        {
            get
            {
                return new RelayCommand(ExecuteSelectResult, CanSelectResult);
            }
        }
        public void ExecuteSelectResult(object parameter)
        {
            Console.WriteLine("inside executeselectresult");
            Console.WriteLine("selected item title: " + SelectedResult.Title);
            
        }

        public bool CanSelectResult(object parameter)
        {
            if (SelectedResult != null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
