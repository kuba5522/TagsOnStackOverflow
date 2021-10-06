using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace TagsOnStackOverflow.Models
{
    public class Http
    {
        private HttpClientHandler _handler;
        private HttpClient _client;

        public Http()
        {
             _handler = new HttpClientHandler()
             {
                 AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
             };
             _client = new HttpClient(_handler);
        }

        public async Task<string> GetResponse(string url)
        {
            //setup http client
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //make request
            string response = await _client.GetStringAsync(url);

            return response;
        }
    }
}