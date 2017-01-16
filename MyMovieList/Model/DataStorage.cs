using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
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

        private static ObservableCollection<string> _Genres = new ObservableCollection<string>();
        public static ObservableCollection<string> Genres
        {
            get { return _Genres; }
        }

        #region ReadAndSaveFile
        public static void SaveListToFile(string fileName)
        {
            if (fileName == null)
            {
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            //new JsonSerializerSettings() {DefaultValueHandling = DefaultValueHandling.Ignore };
            serializer.NullValueHandling = NullValueHandling.Ignore;
            

            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
                foreach (var mov in Movies)
                {
                    serializer.Serialize(writer, mov);
                    sw.Write(Environment.NewLine);
                }
        }

        public static void OpenListFromFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName) || fileName == "N/A")
            {
                return;
            }

            EmptyStorage();

            // read from file with deserialize json
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonTextReader reader = new JsonTextReader(file);
                reader.SupportMultipleContent = true;
                while (true)
                {
                    if (!reader.Read()) { break; }
                    JsonSerializer serializer = new JsonSerializer();
                    Movie movie = serializer.Deserialize<Movie>(reader);
                    Movies.Add(movie);
                    UpdateGenres(movie.Genre);
                }
            }
        }

        public static void EmptyStorage()
        {
            // fucks up the binding
            //Movies = new ObservableCollection<Movie>();
            //_Genres = new ObservableCollection<string>();
            //Genres.Add("All");

            //remove all movies from observable collection
            for (int i = Movies.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine("removing " + Movies[i].Title);
                Movies.RemoveAt(i);
            }

            //remove all genres from observable collection. keep the first "All".
            for (int i = Genres.Count - 1; i >= 0; i--)
            {
                //if (Genres[i] == "All")
                //{
                //    continue;
                //}
                //Console.WriteLine("removing " + Genres[i]);
                Genres.RemoveAt(i);
            }

            Genres.Add("All");
        }
        #endregion

        public static bool MovieExistsInCollection(Movie movie)
        {
            if (movie == null)
            {
                return false;
            }
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
            if (ID == null)
            {
                return false;
            }
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

        public static void RemoveMovieFromList(Movie movie)
        {
            if (MovieExistsInCollection(movie))
            {
                foreach (var mov in Movies)
                {
                    if (mov.imdbID == movie.imdbID)
                    {
                        Movies.Remove(mov);
                        CurrentMovie = null;
                        return;
                    }
                }
            }
        }

        private static bool GenreIsInList(string genre)
        {
            return Genres.Any(s => s.Contains(genre.Trim()));
        }

        public static void UpdateGenres(string genre)
        {
            bool genreAdded = false;
            string[] s = genre.Split(',');
            foreach (var gen in s)
            {
                if (!GenreIsInList(gen))
                {
                    Genres.Add(gen.Trim());
                    genreAdded = true;
                }
            }
            if (genreAdded)
            {
                SortGenres();
            }
        }

        public static void UpdateGenres(Movie movie)
        {
            bool genreAdded = false;
            string[] s = movie.Genre.Split(',');
            foreach (var gen in s)
            {
                if (!GenreIsInList(gen))
                {
                    Genres.Add(gen.Trim());
                    genreAdded = true;
                }
            }
            if (genreAdded)
            {
                SortGenres();
            }
        }

        public static void UpdateGenresFull()
        {
            foreach (var mov in Movies)
            {
                string[] s = mov.Genre.Split(',');
                foreach (var gen in s)
                {
                    if (!GenreIsInList(gen))
                    {
                        Genres.Add(gen.Trim());
                    }
                }
            }
            SortGenres();
        }

        private static void SortGenres()
        {
            List<string> tempSort = new List<string>();
            for (int i = 1; i < Genres.Count; i++)
            {
                tempSort.Add(Genres[i]);
            }
            tempSort.Sort();
            for(int i = 0; i < tempSort.Count; i++)
            {
                Genres[i + 1] = tempSort[i];
            }
        }

        public async static Task RedownloadAllMovies()
        {
            if (Movies.Count == 0)
            {
                return;
            }
            foreach (var mov in Movies)
            {
                Debug.WriteLine("updating: ", mov.Title);
                await mov.LoadDataWithID(mov.imdbID);
                await Task.Delay(1000);
            }
        }
    }
}
