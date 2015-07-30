using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using MyMovieList.Model;
using MyMovieList.Utilities;

namespace MyMovieList.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel()
        {
            System.Console.WriteLine("creating new searchviewmodel");
        }
        //private SearchParser searchParser;

        private string _NewSearch = "";
        public string NewSearch
        {
            get
            {
                return _NewSearch;
            }
            set
            {
                _NewSearch = value;
                onPropertyChanged("NewSearch");
                SearchStatus = "Searching for " + _NewSearch;
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
                return new RelayCommand();
            }
        }

        public void ExecuteNewSearch(object parameter)
        {

        }

        public bool CanNewSearch(object parameter)
        {
            return true;
        }
        #endregion
    }
}
