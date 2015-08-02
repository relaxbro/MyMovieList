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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MyMovieList.ViewModel;
using MyMovieList.Utilities;


namespace MyMovieList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel vm { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel();
            this.DataContext = vm;
            Messenger.Default.Register<OnPropertyChangedMessage>(this, (action) => vm.RecieveMessage(action));

        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            vm.SearchForNew(this);
        }
    }
}
