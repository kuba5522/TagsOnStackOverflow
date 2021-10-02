using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using TagsOnStackOverflow.Models;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TagsOnStackOverflow.Controllers
{
    public class HomeController : Controller
    {
        //list of tags
        private List<Tag> _tags;    

        public HomeController()
        {
            _tags = new List<Tag>();
        }

        public async Task<ActionResult> Index()
        {
            //10 pages of items
            for (int i = 0; i < 10; i++)
            {
                string response = await HttpHandlerAsync(i + 1);
                if (response != null)
                    JsonDecode(response, i);
            }
            //calculate popularity of tags
            int sum = _tags.Sum(x => x.NumberOfOccurrences);
            _tags.ForEach(x => x.Popularity = string.Format("{0:P3}", (double) x.NumberOfOccurrences / sum));
            return View(_tags);
        }

        private async Task<string> HttpHandlerAsync(int page)
        {
            string apiUrl = ($"https://api.stackexchange.com/2.3/tags?page=" + page + "&pagesize=100&order=desc&sort=popular&site=stackoverflow");
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpClient client = new HttpClient(handler);

            //setup http client
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //make request
            string response = await client.GetStringAsync(apiUrl);

            return response;
        }

        private void JsonDecode(string response, int page)
        {
            JObject json = JObject.Parse(response);
            for (int j = 0; j < 100; j++)   //foreach item in json
            {
                string name = (string)json.SelectToken("items[" + j + "].name");
                int count = (int)json.SelectToken("items[" + j + "].count");
                _tags.Add(new Tag()
                {
                    ID = page * 100 + j + 1,   //1-1000
                    Name = name,
                    NumberOfOccurrences = count,
                    Popularity = string.Empty
                }
                );
            }
        }
    }
}
