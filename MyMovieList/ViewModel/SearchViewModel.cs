using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;
using GalaSoft.MvvmLight.Messaging;
using MyMovieList.View;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        ObservableCollection<OmdbSearchResult> _OmdbSearchResults = new ObservableCollection<OmdbSearchResult>();
        MessageBoxManager _messageBoxManager = new MessageBoxManager();

        //private IObservable<string> searchObservable;
        //DateTime timeAtLastSearchUpdate = DateTime.UtcNow;



        public SearchViewModel()
        {
            Debug.WriteLine("creating new searchviewmodel");

            //var searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
            //    .Where(e => e.EventArgs.PropertyName == "NewSearchTerm")
            //    .Select(_ => this.NewSearchTerm)
            //    .Throttle(TimeSpan.FromMilliseconds(300))
            //    .Subscribe(e => DoNewSearch());
            //    .SelectMany(async _ => await DoNewSearch())

        }

        //private IObservable<string> searchObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
        //    .Where(e => e.EventArgs.PropertyName == "NewSearchTerm")
        //    .Select(NewSearchTerm)
        //    .Throttle(TimeSpan.FromMilliseconds(300))
        //    .Subscribe(DoNewSearch);

        //private IObservable<string> searchObservable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
        //    h => this.PropertyChanged() += h, h => PropertyChanged -= h);



        //public event PropertyChangedEventHandler SearchPropertyChanged;
        //private event PropertyChangedEventHandler privateSearchPropertyChanged;    


        //private IObservable<string> searchObservable = Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventHandler>(
        //    h => sw.searchTextBox.TextChanged += h,
        //    h => sw.searchTextBox.TextChanged -= h)
        //    .Select(XamlGeneratedNamespace => sw.searchTextBox.Text)
        //    .Throttle(TimeSpan.FromMilliseconds(300));


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
                // binding uses delay. Not the best way of doing this. Should use reactive extensions.
                _NewSearchTerm = value;
                onPropertyChanged("NewSearchTerm");
                DoNewSearch();

                // this is a baaaad way of doing this
                //if (DateTime.UtcNow - timeAtLastSearchUpdate > TimeSpan.FromMilliseconds(1000))
                //{
                //    Debug.WriteLine("Inside the shit going down");
                //    DoNewSearch();
                //    timeAtLastSearchUpdate = DateTime.UtcNow;
                //}

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
                Debug.WriteLine("Searchstatus: " + _SearchStatus);
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

        public async Task<string> UseSearchParser(string search)
        {
            SearchParser searchParser = new SearchParser();
            return await searchParser.Search(search);
        }

        #region Search
        public async void DoNewSearch()
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
            Debug.WriteLine("NewSearchTerm: " + NewSearchTerm);

            SearchStatus = "Searching for " + NewSearchTerm;
            string searchResult = await UseSearchParser(NewSearchTerm);
            Debug.WriteLine(searchResult);

            if (searchResult.StartsWith(";"))
            {
                SearchStatus = searchResult.Substring(1);
                return;
            }


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
                    Debug.WriteLine("Fuck observablecollection is null");
                }

            }
            else
            {
                Debug.WriteLine("no valid results");
                SearchStatus = "No results found";
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
        public async void ExecuteSelectAndClose(object parameter)
        {
            if (DataStorage.MovieExistsInCollection(SelectedResult.imdbID))
            {
                CurrentMovie = DataStorage.GetMovieWithID(SelectedResult.imdbID);
            }
            else
            {
                try
                {
                    //CurrentMovie = new Movie(SelectedResult.imdbID);
                    // cant use constructor because of async inside
                    var mov = new Movie();
                    await mov.LoadDataWithID(SelectedResult.imdbID);
                    CurrentMovie = mov;
                }
                catch
                {
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
            }

            Debug.WriteLine("after update " + CurrentMovie.Title);
            
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
