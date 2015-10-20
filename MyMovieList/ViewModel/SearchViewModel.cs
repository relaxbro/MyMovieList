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
using GalaSoft.MvvmLight.Messaging;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        ObservableCollection<OmdbSearchResult> _OmdbSearchResults = new ObservableCollection<OmdbSearchResult>();
        MessageBoxManager _messageBoxManager = new MessageBoxManager();

        public SearchViewModel()
        {
            System.Console.WriteLine("creating new searchviewmodel");
        }


        //public ObservableCollection<Movie> Movies
        //{
        //    get
        //    {
        //        return DataStorage.Movies;
        //    }
        //    set
        //    {
        //        DataStorage.Movies = value;
        //        onPropertyChanged("Movies");
        //    }
        //}

        public Movie CurrentMovie
        {
            get
            {
                return DataStorage.CurrentMovie;
            }
            set
            {
                DataStorage.CurrentMovie = value;
                onPropertyChanged("CurrentMovie");
            }
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
                System.Console.WriteLine("Searchstatus: " + _SearchStatus);
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
        public void DoNewSearch()
        {
            if (string.IsNullOrEmpty(NewSearchTerm))
            {
                SearchStatus = "Empty search";
                return;
            }
            else if (NewSearchTerm.Length == 1)
            {
                SearchStatus = "Provide more than one character";
                return;
            }

            _OmdbSearchResults.Clear();
            //System.Console.WriteLine("inside ExecuteNewSearch");
            System.Console.WriteLine("NewSearchTerm: " + NewSearchTerm);

            SearchStatus = "Searching for " + NewSearchTerm;
            string searchResult = UseSearchParser(NewSearchTerm);
            System.Console.WriteLine(searchResult);


            string[] searchResultSplit = searchResult.Split(':');
            if (searchResultSplit[0] == "{\"Title\"")
            {
                JObject obj = JObject.Parse(searchResult);
                _OmdbSearchResults.Add(new OmdbSearchResult((string)obj["Title"], (string)obj["Year"], (string)obj["imdbID"]));
                SearchStatus = "Found 1 movie.";

            }
            else if (searchResultSplit[0] == "{\"Search\"")
            {
                string searchResultTrimmed = "[" + searchResult.Split('[', ']')[1] + "]";

                JSonHelper jsonHelper = new JSonHelper();
                OmdbSearchResults = jsonHelper.ConvertJSonToObject<ObservableCollection<OmdbSearchResult>>(searchResultTrimmed);

                if (_OmdbSearchResults.Count == 1)
                {
                    SearchStatus = "Found " + _OmdbSearchResults.Count + " movie.";
                }
                else
                {
                    SearchStatus = "Found " + _OmdbSearchResults.Count + " movies.";
                }

                if (_OmdbSearchResults == null)
                {
                    Console.WriteLine("Fuck observablecollection is null");
                }

            }
            else
            {
                Console.WriteLine("no valid results");
                //SearchStatus = "No results found: " + searchResult;
                SearchStatus = "Movie not found";
            }
        }

        public ICommand NewSearch
        {
            get
            {
                return new RelayCommand(ExecuteNewSearch, CanNewSearch);
            }
        }

        public void ExecuteNewSearch(object parameter)
        {
            DoNewSearch();

            //_OmdbSearchResults.Clear();
            ////System.Console.WriteLine("inside ExecuteNewSearch");
            //System.Console.WriteLine("NewSearchTerm: " + NewSearchTerm);

            //SearchStatus = "Searching for " + NewSearchTerm;
            //string searchResult = UseSearchParser(NewSearchTerm);
            //System.Console.WriteLine(searchResult);


            //string[] searchResultSplit = searchResult.Split(':');
            //if (searchResultSplit[0] == "{\"Title\"")
            //{ 
            //    JObject obj = JObject.Parse(searchResult);
            //    _OmdbSearchResults.Add(new OmdbSearchResult((string)obj["Title"], (string)obj["Year"], (string)obj["imdbID"]));
            //    SearchStatus = "Found 1 movie.";

            //}
            //else if (searchResultSplit[0] == "{\"Search\"")
            //{
            //    string searchResultTrimmed = "[" + searchResult.Split('[', ']')[1] +"]";

            //    JSonHelper jsonHelper = new JSonHelper();
            //    OmdbSearchResults = jsonHelper.ConvertJSonToObject<ObservableCollection<OmdbSearchResult>>(searchResultTrimmed);
            //    SearchStatus = "Found " + _OmdbSearchResults.Count + " movies.";

            //    if (_OmdbSearchResults == null)
            //    {
            //        Console.WriteLine("Fuck observablecollection is null");
            //    }

            //}
            //else
            //{
            //    Console.WriteLine("no valid results");
            //    SearchStatus = "No results found: " + searchResult;
            //}

        }

        public bool CanNewSearch(object parameter)
        {
            return true;
        }
        #endregion


        #region SelectAndClose
        public ICommand SelectAndCloseCommand
        {
            get
            {
                return new RelayCommand(ExecuteSelectAndClose, CanSelectAndClose);
            }
        }
        public void ExecuteSelectAndClose(object parameter)
        {
            if (DataStorage.MovieExistsInCollection(SelectedResult.imdbID))
            {
                CurrentMovie = DataStorage.GetMovieWithID(SelectedResult.imdbID);
            }
            else
            {
                try
                {
                    CurrentMovie = new Movie(SelectedResult.imdbID);
                }
                catch (Exception e)
                {
                    //System.Console.WriteLine(e);
                    var msgBoxResult = _messageBoxManager.ShowMessageBox(
                                                    "Failed parsing Json. Data might be corrupt. Go back to main window?",
                                                    "Confirmation",
                                                    System.Windows.MessageBoxButton.YesNo);

                    if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
                    {
                        CloseWindowAction();
                    }
                    else if (msgBoxResult == System.Windows.MessageBoxResult.No)
                    {
                        return;
                    }
                    return;
                }
                //Movies.Add(CurrentMovie);
            }

            Console.WriteLine("after update " + CurrentMovie.Title);
            
            // TODO create load method

            UpdateCurrentMovie();
            CloseWindowAction();
        }

        public bool CanSelectAndClose(object parameter)
        {
            if (SelectedResult != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        public Action CloseWindowAction { get; set; }

        private object UpdateCurrentMovie()
        {
            Console.WriteLine("inside svm trying to send message");
            var msg = new OnPropertyChangedMessage() { Property = "CurrentMovie" };
            Messenger.Default.Send<OnPropertyChangedMessage>(msg);
            return null;
        }
    }
}
