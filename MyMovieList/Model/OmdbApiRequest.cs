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
        private string URLBase = "http://www.omdbapi.com/?";

        public string RequestWithID(string imdbID)
        {
            string URLfull = URLBase + "i=" + imdbID + "&plot=full&r=json";
            WebRequest request = WebRequest.Create(URLfull);
            WebResponse response = request.GetResponse();
            System.Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            //System.Console.WriteLine(responseFromServer);

            reader.Close();
            response.Close();

            return responseFromServer;
        }

        public string RequestWithSearch(string search)
        {
            string URLfull = URLBase + "s=" + search + "&r=json";
            //string URLfull = URLBase + 
            WebRequest request = WebRequest.Create(URLfull);
            WebResponse response = request.GetResponse();
            System.Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            //System.Console.WriteLine(responseFromServer);

            reader.Close();
            response.Close();

            return responseFromServer;
        }
    }
}
