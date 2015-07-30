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

namespace MyMovieList.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        //public SearchViewModel ViewModel { get; set; }
        //public SearchViewModel ViewModel;

        public SearchWindow()
        {
            //ViewModel = new SearchViewModel();

            //DataContext = ViewModel;

            InitializeComponent();
        }

        private void SearchWindowSearchButtonClick(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("test NewSearch" + (SearchViewModel)DataContext.NewSearch);
            //string result = vm.UseSearchParser(vm.NewSearch);
            //System.Console.WriteLine(result);
        }

        private void SearchWindowCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
