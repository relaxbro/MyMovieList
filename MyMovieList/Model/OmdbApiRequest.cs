using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace MyMovieList.Model
{
    class OmdbApiRequest
    {
        private int _webTimeOut = 2000;
        private string URLBase = "http://www.omdbapi.com/?";

        //public string RequestWithID(string imdbID)
        public async Task<string> RequestWithID(string imdbID)
        {
            string URLfull = URLBase + "i=" + imdbID + "&plot=full&r=json&tomatoes=true";
            WebRequest request = WebRequest.Create(URLfull);
            request.Timeout = _webTimeOut;
            WebResponse response = null;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch
            {
                return ";Search timed out";
            }

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return responseFromServer;
        }

        //public string RequestWithSearch(string search, int year = 0)
        public async Task<string> RequestWithSearch(string search, int year = 0)
        {
            string URLfull;
            if (year != 0)
            {
                URLfull = URLBase + "s=" + search + "&type=movie&r=json&y=" + year.ToString();
            }
            else
            {
                URLfull = URLBase + "s=" + search + "&type=movie&r=json";
            }

            WebRequest request = WebRequest.Create(URLfull);
            request.Timeout = _webTimeOut;
            WebResponse response = null;
            try
            {
                response = await request.GetResponseAsync();
            }
            catch
            {
                return ";Search timed out";
            }

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return responseFromServer;
        }
    }
}
