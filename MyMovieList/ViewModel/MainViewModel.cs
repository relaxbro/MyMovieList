using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MyMovieList.Model;


namespace MyMovieList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        ObservableCollection<Movie> _Movies = new ObservableCollection<Movie>();
        //Movie _CurrentMovie = new Movie("tt0478970");
        Movie _CurrentMovie;

        public ObservableCollection<Movie> Movies
        {
            get
            {
                return _Movies;
            }
            set
            {
                _Movies = value;
                onPropertyChanged("Movies");
            }
        }

        public Movie CurrentMovie
        {
            get
            {
                return _CurrentMovie;
            }
            set
            {
                _CurrentMovie = value;
                onPropertyChanged("CurrentMovie");
            }
        }

        public void SearchForNew(MainWindow mainWindow)
        {
            View.SearchWindow searchWindow = new View.SearchWindow(CurrentMovie);
            searchWindow.Owner = mainWindow;
            searchWindow.Show();
        }
    }
}
