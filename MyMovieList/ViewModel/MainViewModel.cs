using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMovieList.Model;


namespace MyMovieList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public void SearchForNew(MainWindow mainWindow)
        {
            System.Console.WriteLine("jalla0");
            View.SearchWindow searchWindow = new View.SearchWindow();
            System.Console.WriteLine("jalla inne i searchfornew");
            searchWindow.Owner = mainWindow;
            searchWindow.Show();
        }
    }
}
