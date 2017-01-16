using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyMovieList.Model
{
    public class SearchParser
    {
        private OmdbApiRequest omdb = new OmdbApiRequest();
        private int imdbIDLength = 9;

        //public string Search(string search)
        public async Task<string> Search(string searchBeforeTrim)
        {
            string search = searchBeforeTrim.Trim();
            // check if full imdb link and search
            if (search.Contains("http") || search.Contains("imdb.com"))
            {
                string searchTerm = "";
                string[] words = search.Split('/');
                foreach (var word in words)
                {
                    if (word.Length == imdbIDLength && word[0] == 't' && word[1] == 't')
                    {
                        searchTerm = word;
                        break;
                    }
                }
                return  await omdb.RequestWithID(searchTerm);
            }

            // search using ID only
            if (search.Length == imdbIDLength && search[0] == 't' && search[1] == 't')
            {
                return await omdb.RequestWithID(search);
            }

            // search using title (and year)
            if (search.Contains(';'))
            {
                string[] words = search.Split(';');
                int year;
                if (Int32.TryParse(words[1].Trim(), out year))
                {
                    return await omdb.RequestWithSearch(words[0], year);
                }
            }
            return await omdb.RequestWithSearch(search);
            
        }
    }
}
