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
        private static Random rnd = new Random();
        private SettingsHandler settingsHandler = new SettingsHandler();

        MessageBoxManager _messageBoxManager = new MessageBoxManager();

        public MainViewModel()
        {
            settingsHandler.LoadSettings();
            
            if (settingsHandler.OpenPreviousOnStartup)
            {
                DataStorage.OpenListFromFile(settingsHandler.PreviousFile);
                currentSaveFileName = settingsHandler.PreviousFile;
                StatusString = "File loaded: " + currentSaveFileName;
                GetRandomMovie();
            }

            // connect the icollectionview to observable collection movie
            _movieCollectionView = CollectionViewSource.GetDefaultView(Movies);
            // connect the icollectionview to the observable collection genre
            _genresCollectionView = CollectionViewSource.GetDefaultView(Genres);
            
            if (Genres.Count == 0)
            {
                Genres.Add("All");
            }
            
            // sort according to title
            _movieCollectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            // create filter to filter the collection
            _movieCollectionView.Filter = FullFilter;

            //DataStorage.Movies.Add(new Movie("tt3832914"));
            //DataStorage.Movies.Add(new Movie("tt0478970"));
            //DataStorage.Movies.Add(new Movie("tt0270919"));
            //DataStorage.Movies.Add(new Movie("tt0241527"));
            //DataStorage.Movies.Add(new Movie("tt0330373"));
            //DataStorage.Movies.Add(new Movie("tt0417741"));
            //DataStorage.Movies.Add(new Movie("tt0304141"));

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
                _movieCollectionView.SortDescriptions[0] = new SortDescription("MyRatingInt", ListSortDirection.Ascending);
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
            onPropertyChanged("NumberOfFilteredMovies");
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
            else if (String.IsNullOrEmpty(SelectedGenre)){
                return true;
            }
            else
            {
                //if (string.IsNullOrEmpty(SelectedGenre))
                //{
                //    SelectedGenre = "All";
                //    return true;
                //}
                Movie movie = item as Movie;
                //Console.WriteLine("Selected genre; " + SelectedGenre);
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
        private bool _ShuttingDown = false;
        private bool _HasUnsavedChanges = false;
        public bool HasUnsavedChanges
        {
            get { return _HasUnsavedChanges; }
            set { _HasUnsavedChanges = value; }
        }

        public ICollectionView MovieCollectionView
        {
            get
            {
                return _movieCollectionView;
            }
        }

        public int NumberOfFilteredMovies
        {
            get
            { 
                System.Console.WriteLine("filtered count " + MovieCollectionView.OfType<Movie>().Count());
                return MovieCollectionView.OfType<Movie>().Count();
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
                UpdateCounter();
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
                UpdateCounter();
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
                UpdateCounter();
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
                UpdateCounter();
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

        private void UpdateCounter()
        {
            onPropertyChanged("NumberOfFilteredMovies");
            return;
        }

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
            //Console.WriteLine("inside executeUpdateMovieAsSeen");
            //SelectedMovie.Seen = true;

            string currentDate = DateTime.Today.ToString("yyyy.MM.dd");

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
            HasUnsavedChanges = true;
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
            if (!DataStorage.MovieExistsInCollection(SelectedMovie))
            {
                //Console.WriteLine("movie not in collection. adding");
                StatusString = "Added to list: " + SelectedMovie.Title;
                SelectedMovie.DateAddedToList = DateTime.Today.ToString("yyyy.MM.dd");
                Movies.Add(SelectedMovie);
                HasUnsavedChanges = true;
                DataStorage.UpdateGenres(SelectedMovie);
                onPropertyChanged("Genres");
                UpdateCounter();
                
            }
            else
            {
                //Console.WriteLine("movie allready in collection. not adding.");
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
            UpdateCounter();
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

        private void ExecuteOpenURL(object parameter)
        {
            //Console.WriteLine("inside ExecuteOpenURL");
            try
            {
                System.Diagnostics.Process.Start(SelectedMovie.imdbURL);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
                return;
            }
        }

        private bool CanOpenURL(object parameter)
        {
            if (SelectedMovie == null)
            {
                return false;
            }
            return true;
        }
        public ICommand OpenUrlGitHub
        {
            get
            {
                return new RelayCommand(ExecuteOpenUrlGitHub, CanOpenUrlGitHub);
            }
        }

        private void ExecuteOpenUrlGitHub(object parameter)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/relaxbro/MyMovieList");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return;
            }
        }

        private bool CanOpenUrlGitHub(object parameter)
        {
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
        #region NewFileCommand
        public ICommand NewFileCommand
        {
            get { return new RelayCommand(ExecuteNewFile, CanNewFile); }
        }

        private void ExecuteNewFile(object paramater)
        {
            NewFile();
        }

        private bool CanNewFile(object paramter) { return true; }

        private void NewFile()
        {
            // check if current list has unsaved changes
            if (HasUnsavedChanges)
            {
                var msgBoxResult = _messageBoxManager.ShowMessageBox(
                                                    "Unsaved changes, save before creating new file?",
                                                    "Confirmation",
                                                    System.Windows.MessageBoxButton.YesNoCancel);
                if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.No)
                {

                }
            }

            // empty current list
            DataStorage.EmptyStorage();
            UpdateCounter();

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".json";
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = false;
            fileDialog.Filter = "JSON (*.json)|*.json";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                currentSaveFileName = fileDialog.FileName;
                //Console.WriteLine("chosen file " + currentSaveFileName);
                StatusString = "New list file: " + currentSaveFileName;
            }
        }
        #endregion

        #region OpenFileCommand
        public ICommand OpenFileCommand
        {
            get { return new RelayCommand(ExecuteOpenFile, CanOpenFile); }
        }

        private void ExecuteOpenFile(object paramater)
        {
            OpenFile();
        }

        private bool CanOpenFile(object paramter) { return true; }

        private void OpenFile()
        {
            // check if current list has unsaved changes
            if (HasUnsavedChanges)
            {
                var msgBoxResult = _messageBoxManager.ShowMessageBox(
                                                    "Unsaved changes, save before opening new file?",
                                                    "Confirmation",
                                                    System.Windows.MessageBoxButton.YesNoCancel);
                if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.No)
                {

                }
            }

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".json";
            fileDialog.Filter = "JSON (*.json)|*.json|All files (*.*)|*.*";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                currentSaveFileName = fileDialog.FileName;
                DataStorage.OpenListFromFile(currentSaveFileName);
                HasUnsavedChanges = false;
                StatusString = "Current file: " + currentSaveFileName;
                settingsHandler.PreviousFile = currentSaveFileName;
                UpdateCounter();

                if (SelectedMovie.Title == "Dummy movie")
                {
                    GetRandomMovie();
                }
            }
        }
        #endregion

        #region SaveFileCommand
        public ICommand SaveFileCommand
        {
            get { return new RelayCommand(ExecuteSaveFile, CanSaveFile); }
        }

        private void ExecuteSaveFile(object paramater)
        {
            SaveFile();
        }

        private bool CanSaveFile(object paramter) { return true; }

        private void SaveFile()
        {
            // check if currentSaveFileName is a correct file
            if (string.IsNullOrEmpty(currentSaveFileName))
            {
                SaveFileAs();
            }
            else
            {
                DataStorage.SaveListToFile(currentSaveFileName);
                HasUnsavedChanges = false;
                StatusString = "Saved to file: " + currentSaveFileName;
            }
        }
        #endregion

        #region NewFileCommand
        public ICommand SaveFileAsCommand
        {
            get { return new RelayCommand(ExecuteSaveFileAs, CanSaveFileAs); }
        }

        private void ExecuteSaveFileAs(object paramater)
        {
            SaveFileAs();
        }

        private bool CanSaveFileAs(object paramter) { return true; }

        private void SaveFileAs()
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
                //Console.WriteLine("chosen file " + currentSaveFileName);
                DataStorage.SaveListToFile(currentSaveFileName);
                HasUnsavedChanges = false;
                StatusString = "Saved to file: " + currentSaveFileName;
            }
        }
        #endregion
        #endregion

        #region Exit
        public ICommand ExitApplicationCommand
        {
            get { return new RelayCommand(ExecuteExitApplication, CanExitApplication); }
        }

        private void ExecuteExitApplication(object parameter)
        {
            //System.Console.WriteLine("inside ExecuteExitApplication() start");
            // to avoid getting the dialog twice. this because current.shutdown() fires event which is the same as exiting with red x
            if (_ShuttingDown)
            {
                return;
            }
            settingsHandler.SaveSettings();
            if (HasUnsavedChanges)
            {
                var msgBoxResult = _messageBoxManager.ShowMessageBox(
                                                    "Unsaved changes, save before closing?",
                                                    "Confirmation",
                                                    System.Windows.MessageBoxButton.YesNoCancel);
                if (msgBoxResult == System.Windows.MessageBoxResult.Yes)
                {
                    _ShuttingDown = true;
                    SaveFile();
                    System.Windows.Application.Current.Shutdown();
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (msgBoxResult == System.Windows.MessageBoxResult.No)
                {
                    _ShuttingDown = true;
                    System.Windows.Application.Current.Shutdown();
                    return;
                }
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
            //System.Console.WriteLine("inside ExecuteExitApplication() end");

        }

        private bool CanExitApplication(object paramater)
        {
            return true;
        }

        public void ExitApplication()
        {
            ExecuteExitApplication(null);
        }
        #endregion

        #region GetRandomMovie
        public ICommand SelectRandomMovieFromList
        {
            get { return new RelayCommand(ExecuteSelectRandomMovieFromList, CanSelectRandomMovieFromList); }
        }

        private void ExecuteSelectRandomMovieFromList(object parameter)
        {
            GetRandomMovie();
        }

        private bool CanSelectRandomMovieFromList(object parameter)
        {
            // check if current view is empty
            if (MovieCollectionView.OfType<Movie>().Count() > 1)
            {
                return true;
            }
            return false;
        }

        private void GetRandomMovie()
        {   
            // kinda ugly. hopefully there is a more efficient way to find random from filtered icollectionview
            bool found = false;
            int index = 0;
            while (!found)
            {
                index = rnd.Next(Movies.Count);
                if (FullFilter(Movies[index]) && (Movies[index].imdbID != SelectedMovie.imdbID))
                {
                    found = true;
                }
            }
            SelectedMovie = Movies[index];
            //SelectedMovie = Movies[rnd.Next(Movies.Count-1)];
            StatusString = "Random movie: " + SelectedMovie.Title;
        }
        #endregion

        public object RecieveMessage(OnPropertyChangedMessage action)
        {
            SelectedMovie = CurrentMovie;
            return null;
        }
    }
}
