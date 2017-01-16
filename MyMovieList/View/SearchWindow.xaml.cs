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
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using MyMovieList.ViewModel;
using MyMovieList.Model;

namespace MyMovieList.View
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchViewModel vm { get; set; }

        public SearchWindow()
        {           
            InitializeComponent();
            //SearchViewModel vm = new SearchViewModel();
            vm = new SearchViewModel();
            this.DataContext = vm;
            if (vm.CloseWindowAction == null)
            {
                vm.CloseWindowAction = new Action(() => this.Close());
            }
        }

        

        private void SearchWindowSearchEnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Console.WriteLine("inside SearchWindowSearchEnterKeyDown");
                vm.DoNewSearch();
            }
        }

        private void SearchWindowCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
