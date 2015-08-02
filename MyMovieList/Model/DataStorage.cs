using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MyMovieList.Model;

namespace MyMovieList.Model
{
    public static class DataStorage
    {
        private static ObservableCollection<Movie> _Movies = new ObservableCollection<Movie>();
        public static ObservableCollection<Movie> Movies
        {
            get { return _Movies; }
            set { _Movies = value; }
        }

        private static Movie _CurrentMovie = new Movie();
        public static Movie CurrentMovie
        {
            get { return _CurrentMovie; }
            set { _CurrentMovie = value; }
        }

        public static bool MovieExistsInCollection(Movie movie)
        {
            foreach (var mov in Movies)
            {
                if (movie.imdbID == mov.imdbID)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool MovieExistsInCollection(string ID)
        {
            foreach (var mov in Movies)
            {
                if (ID == mov.imdbID)
                {
                    return true;
                }
            }
            return false;
        }

        public static Movie GetMovieWithID(string ID)
        {
            foreach (var mov in Movies)
            {
                if (ID == mov.imdbID)
                {
                    return mov;
                }
            }
            return null;
        }
    }
}
