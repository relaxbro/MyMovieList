using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Model
{
    public class SearchParser
    {
        private OmdbApiRequest omdb = new OmdbApiRequest();
        private int imdbIDLength = 9;

        public string Search(string search)
        {
            if (search.Length == imdbIDLength && search[0] == 't' && search[1] == 't')
            {
                System.Console.WriteLine("Searching with imdbID");
                return omdb.RequestWithID(search);
            }
            else
            {
                System.Console.WriteLine("Searching general");
                return omdb.RequestWithSearch(search);
            }
        }
    }
}
