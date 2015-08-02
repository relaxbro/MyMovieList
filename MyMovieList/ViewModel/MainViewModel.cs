using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MyMovieList.Model;
using MyMovieList.Utilities;


namespace MyMovieList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //ObservableCollection<Movie> _Movies = new ObservableCollection<Movie>();
        //ObservableCollection<Movie> _CurrentMovies = new ObservableCollection<Movie>();
        //private Movie _CurrentMovie = new Movie("tt0478970");
        
        public MainViewModel()
        {
            //_CurrentMovies.Add(_CurrentMovie);
            Console.WriteLine("creating new MainViewModel");
            DataStorage.Movies.Add(new Movie("tt0478970"));
            DataStorage.Movies.Add(new Movie("tt0270919"));
        }


        public ObservableCollection<Movie> Movies
        {
            get
            {
                return DataStorage.Movies;
            }
            set
            {
                DataStorage.Movies = value;
                onPropertyChanged("Movies");
            }
        }

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

        private Movie _SelectedMovie;
        public Movie SelectedMovie
        {
            get { return _SelectedMovie; }
            set
            {
                _SelectedMovie = value;
                CurrentMovie = _SelectedMovie;
                onPropertyChanged("SelectedMovie");
            }
        }

        #region AddToList
        public ICommand AddToList
        {
            get
            {
                return new RelayCommand(ExecuteAddToList, CanAddToList);
            }
        }

        public void ExecuteAddToList(object parameter)
        {
            //Console.WriteLine("----------- " + CurrentMovie.Title);
            Console.WriteLine("inside executeAddtoList");
            Console.WriteLine(CurrentMovie.Title);
            //onPropertyChanged("CurrentMovie");
            if (!DataStorage.MovieExistsInCollection(CurrentMovie))
            {
                Console.WriteLine("movie not in collection. adding");
                Movies.Add(CurrentMovie);
                
            }
            else
            {
                Console.WriteLine("movie allready in collectin. not adding.");
            }
        }
        
        public bool CanAddToList(object parameter)
        {
            if (CurrentMovie.imdbID == null)
            {
                return false;
            }
            else if (DataStorage.MovieExistsInCollection(CurrentMovie))
            {
                return false;
            }
            return true;
        }
        #endregion

        public void SearchForNew(MainWindow mainWindow)
        {
            View.SearchWindow searchWindow = new View.SearchWindow();
            searchWindow.Owner = mainWindow;
            searchWindow.Show();
        }


        public object RecieveMessage(OnPropertyChangedMessage action)
        {
            Console.WriteLine("recieving message in mvm: " + action);
            Console.WriteLine("currentmovie: " + CurrentMovie.Title);
            return null;
        }
    }
}
