using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using MyMovieList.Model;
using MyMovieList.Utilities;


namespace MyMovieList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICollectionView _movieCollectionView;
        private ICollectionView _genresCollectionView;
        private string currentSaveFileName;
        private List<string> _SortByListTerms;

        public MainViewModel()
        {
            Console.WriteLine("creating new MainViewModel");
            // connect the icollectionview to observable collection movie
            _movieCollectionView = CollectionViewSource.GetDefaultView(Movies);
            // connect the icollectionview to the observable collection genre
            _genresCollectionView = CollectionViewSource.GetDefaultView(Genres);
            Genres.Add("All");
            
            // sort according to title
            _movieCollectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            // create filter to filter the collection
            _movieCollectionView.Filter = FullFilter;

            DataStorage.Movies.Add(new Movie("tt3832914"));
            DataStorage.Movies.Add(new Movie("tt0478970"));
            DataStorage.Movies.Add(new Movie("tt0270919"));
            DataStorage.Movies.Add(new Movie("tt0241527"));
            DataStorage.Movies.Add(new Movie("tt0330373"));
            DataStorage.Movies.Add(new Movie("tt0417741"));
            DataStorage.Movies.Add(new Movie("tt0304141"));

            //for (int i = 100; i < 150; i++)
            //{
            //    string movieTits = "tt0344" + i.ToString();
            //    Console.WriteLine(movieTits);
            //    DataStorage.Movies.Add(new Movie(movieTits));
            //}

            DataStorage.UpdateGenresFull();

            // Not elegant
            _SortByListTerms = new List<string>();
            SortByListTerms.Add("Title asc");
            SortByListTerms.Add("Title desc");
            SortByListTerms.Add("IMDB desc");
            SortByListTerms.Add("IMDB asc");
            SortByListTerms.Add("My rating desc");
            SortByListTerms.Add("My rating asc");
            SortByListTerms.Add("Year desc");
            SortByListTerms.Add("Year asc");
        }

        #region Sort Collection
        private void SortCollection()
        {
            // ugly AF
            if (SortTerm == "Title asc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("Title", ListSortDirection.Ascending);
            }
            else if (SortTerm == "Title desc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("Title", ListSortDirection.Descending);
            }
            else if (SortTerm == "IMDB desc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("imdbRatingDouble", ListSortDirection.Descending);
            }
            else if (SortTerm == "IMDB asc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("imdbRatingDouble", ListSortDirection.Ascending);
            }
            else if (SortTerm == "My rating desc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("MyRatingInt", ListSortDirection.Descending);
            }
            else if (SortTerm == "My rating asc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("MyRatingInt", ListSortDirection.Descending);
            }
            else if (SortTerm == "Year desc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("Year", ListSortDirection.Descending);
            }
            else if (SortTerm == "Year asc")
            {
                _movieCollectionView.SortDescriptions[0] = new SortDescription("Year", ListSortDirection.Ascending);
            }
        }

        private string _SortTerm = "Title asc";
        public string SortTerm
        {
            get { return _SortTerm; }
            set
            {
                _SortTerm = value;
                SortCollection();
            }
        }

        public List<string> SortByListTerms
        {
            get { return _SortByListTerms; }
        }
        #endregion

        #region FilterForCollection
        private bool FullFilter(object item)
        {
            if (SearchInListFilter(item) && ShowSeenAndNotSeenFilter(item) && GenreFilter(item))
            {
                return true;
            }
            return false;
        }

        private bool GenreFilter(object item)
        {
            if (SelectedGenre == "All")
            {
                return true;
            }
            else
            {
                Movie movie = item as Movie;
                return movie.Genre.Contains(SelectedGenre);
            }
        }

        private bool SearchInListFilter(object item)
        {
            Movie movie = item as Movie;
            //Console.WriteLine("inside SearchListFilter");
            //return movie.Title.Contains(SearchInListString);      // case sensitive
            return movie.Title.IndexOf(SearchInListString, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ShowSeenAndNotSeenFilter(object item)
        {
            Movie movie = item as Movie;
            //Console.WriteLine("inside ShowSeenAndNotSeenFilter, movie.title = ", movie.Title);
            if (ShowSeen && ShowNotSeen)
            {
                return true;
            }
            else if (ShowSeen && !ShowNotSeen)
            {
                if (movie.Seen == true)
                {
                    return true;
                }
                return false;
            }
            else if (!ShowSeen && ShowNotSeen)
            {
                if (movie.Seen == true)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Properties
        public ICollectionView MovieCollectionView
        {
            get
            {
                return _movieCollectionView;
            }
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

        public ICollectionView GenresCollectionView
        {
            get { return _genresCollectionView; }
        }

        public ObservableCollection<string> Genres
        {
            get { return DataStorage.Genres; }
            set
            {
                onPropertyChanged("Genres");
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

        private Movie _SelectedMovie = new Movie();
        public Movie SelectedMovie
        {
            get { return _SelectedMovie; }
            set
            {
                if (value == null)
                {
                    //Console.WriteLine("selected movie is now null.");
                }
                else
                {
                    _SelectedMovie = value;
                    CurrentMovie = _SelectedMovie;
                    onPropertyChanged("SelectedMovie");
                    StatusString = "Selected movie: " + value.Title;
                }
            }
        }

        private string _SelectedGenre = "All";
        public string SelectedGenre
        {
            get { return _SelectedGenre; }
            set
            {
                _SelectedGenre = value;
                onPropertyChanged("SelectedGenre");
                StatusString = "Selected genre: " + SelectedGenre;
                _movieCollectionView.Refresh();
            }
        }

        private bool _ShowSeen = true;
        public bool ShowSeen
        {
            get { return _ShowSeen; }
            set
            {
                _ShowSeen = value;
                onPropertyChanged("ShowSeen");
                _movieCollectionView.Refresh();
            }
        }

        private bool _ShowNotSeen = true;
        public bool ShowNotSeen
        {
            get { return _ShowNotSeen; }
            set
            {
                _ShowNotSeen = value;
                onPropertyChanged("ShowUnseen");
                _movieCollectionView.Refresh();
            }
        }

        private string _SearchInListString = "";
        public string SearchInListString
        {
            get { return _SearchInListString; }
            set
            {
                _SearchInListString = value;
                onPropertyChanged("SearchInListString");
                _movieCollectionView.Refresh();
            }
        }

        private string _StatusString = "";
        public string StatusString
        {
            get { return _StatusString; }
            set
            {
                _StatusString = value;
                onPropertyChanged("StatusString");
            }
        }
        #endregion

        #region UpdateMovieAsSeen
        // ICommand to mark movie as seen
        public ICommand UpdateMovieAsSeen
        {
            get
            {
                return new RelayCommand(ExecuteUpdateMovieAsSeen, CanUpdateMoveAsSeen);
            }
        }

        public void ExecuteUpdateMovieAsSeen(object parameter)
        {
            Console.WriteLine("inside executeUpdateMovieAsSeen");
            //SelectedMovie.Seen = true;

            string currentDate = DateTime.Today.ToString("yyyy/MM/dd");

            if (!SelectedMovie.Seen)
            {
                SelectedMovie.Seen = true;
                SelectedMovie.FirstSeen = currentDate;
                SelectedMovie.LastSeen = currentDate;
            }
            else
            {
                SelectedMovie.LastSeen = currentDate;
            }

            _movieCollectionView.Refresh();
            StatusString = "Updated movie";
        }

        public bool CanUpdateMoveAsSeen(object parameter)
        {
            if (SelectedMovie == null)
            {
                return false;
            }
            else if (DataStorage.MovieExistsInCollection(SelectedMovie))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region AddToList
        // ICommand to add movie to list
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
            Console.WriteLine(SelectedMovie.Title);
            //onPropertyChanged("CurrentMovie");
            if (!DataStorage.MovieExistsInCollection(SelectedMovie))
            {
                Console.WriteLine("movie not in collection. adding");
                StatusString = "Added to list: " + SelectedMovie.Title;
                Movies.Add(SelectedMovie);
                DataStorage.UpdateGenres(SelectedMovie);
                onPropertyChanged("Genres");
                
            }
            else
            {
                Console.WriteLine("movie allready in collection. not adding.");
            }
        }
        
        public bool CanAddToList(object parameter)
        {
            if(SelectedMovie == null)
            {
                return false;
            }
            else if (DataStorage.MovieExistsInCollection(SelectedMovie))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region RedownloadData
        // ICommand to redownload data from IMDB
        public ICommand RedownloadData
        {
            get
            {
                return new RelayCommand(ExecuteRedownloadData, CanRedownloadData);
            }
        }

        public void ExecuteRedownloadData(object parameter)
        {
            //Console.WriteLine("inside ExecuteRedownloadData");
            CurrentMovie.LoadDataWithID(SelectedMovie.imdbID);
            StatusString = "Redownload data for: " + SelectedMovie.Title;
            //Console.WriteLine("redownload of data done");
            
        }

        public bool CanRedownloadData(object parameter)
        {
            return true;
        }
        #endregion

        #region RemoveMovieFromList
        // ICommand for removing movie
        public ICommand RemoveMovieFromList
        {
            get
            {
                return new RelayCommand(ExecuteRemoveMovieFromList, CanRemoveMovieFromList);
            }
        }

        public void ExecuteRemoveMovieFromList(object parameter)
        {
            //Console.WriteLine("inside ExecuteRemoveMovieFromList");
            DataStorage.RemoveMovieFromList(SelectedMovie);
            StatusString = "Removed from list: " + SelectedMovie.Title;
        }

        public bool CanRemoveMovieFromList(object parameter)
        {
            if (DataStorage.MovieExistsInCollection(SelectedMovie))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region OpenURL
        // ICommand for opening the URL
        public ICommand OpenUrl
        {
            get
            {
                return new RelayCommand(ExecuteOpenURL, CanOpenURL);
            }
        }

        public void ExecuteOpenURL(object parameter)
        {
            //Console.WriteLine("inside ExecuteOpenURL");
            System.Diagnostics.Process.Start(SelectedMovie.imdbURL);
        }

        public bool CanOpenURL(object parameter)
        {
            if (SelectedMovie == null)
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

        #region NewOpenAndSaveFile
        public void NewFile()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".json";
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = false;
            fileDialog.Filter = "JSON (*.json)|*.json";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                currentSaveFileName = fileDialog.FileName;
                Console.WriteLine("chosen file " + currentSaveFileName);
                StatusString = "New list file: " + currentSaveFileName;
            }
        }

        public void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".json";
            fileDialog.Filter = "JSON (*.json)|*.json|All files (*.*)|*.*";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                currentSaveFileName = fileDialog.FileName;
                Console.WriteLine("chosen file " + currentSaveFileName);
                DataStorage.OpenListFromFile(currentSaveFileName);
                StatusString = "Current file: " + currentSaveFileName;
            }
        }

        public void SaveFile()
        {
            // check if currentSaveFileName is a correct file
            if (string.IsNullOrEmpty(currentSaveFileName))
            {
                SaveFileAs();
            }
            else
            {
                DataStorage.SaveListToFile(currentSaveFileName);
                StatusString = "Saved to file: " + currentSaveFileName;
            }
        }

        public void SaveFileAs()
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "MovieList";
            saveDialog.DefaultExt = ".json";
            saveDialog.Filter = "JSON (*.json)|.json";

            Nullable<bool> result = saveDialog.ShowDialog();
            if (result == true)
            {
                //string fileName = saveDialog.FileName;
                currentSaveFileName = saveDialog.FileName;
                Console.WriteLine("chosen file " + currentSaveFileName);
                DataStorage.SaveListToFile(currentSaveFileName);
                StatusString = "Saved to file: " + currentSaveFileName;
            }
        }
        #endregion

        public object RecieveMessage(OnPropertyChangedMessage action)
        {
            //Console.WriteLine("recieving message in mvm: " + action);
            //Console.WriteLine("currentmovie: " + CurrentMovie.Title);
            SelectedMovie = CurrentMovie;
            return null;
        }
    }
}
