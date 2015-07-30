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
                return omdb.RequestWithID(search);
            }
            else
            {
                return omdb.RequestWithSearch(search);
            }
        }
    }
}
