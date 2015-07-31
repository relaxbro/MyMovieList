using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyMovieList.ViewModel;
using MyMovieList.Model;

namespace MyMovieList.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        //public SearchViewModel ViewModel { get; set; }
        //public SearchViewModel ViewModel;
        public Movie movieToMainView { get; set; }

        public SearchWindow(Movie currentMovie)
        {
            movieToMainView = currentMovie;
            InitializeComponent();
        }

        private void SearchWindowCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
