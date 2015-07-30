using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        List<Movie> movies = new List<Movie>();

        public SearchViewModel()
        {
            System.Console.WriteLine("creating new searchviewmodel");
        }
        //private SearchParser searchParser;

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
                SearchStatus = "Searching for " + _NewSearchTerm;
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
            System.Console.WriteLine("inside ExecuteNewSearch");
            System.Console.WriteLine("NewSearchTerm: " + NewSearchTerm);
            System.Console.WriteLine(UseSearchParser(NewSearchTerm));

            OmdbApiParseSearch omdbParse = new OmdbApiParseSearch();
        }

        public bool CanNewSearch(object parameter)
        {
            return true;
        }
        #endregion
    }
}
