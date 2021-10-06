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
        private List<Tag> _tags;
        private int _page;
        private int _pageSize;
        private string _order;
        private string _sort;
        private string _site;

        public HomeController()
        {
            _tags = new List<Tag>();
            _page = 1;
            _pageSize = 100;
            _order = "desc";
            _sort = "popular";
            _site = "stackoverflow";
        }

        public async Task<ActionResult> Index()
        {
            Http http = new Http();
            DataProcessing dataProcessing = new DataProcessing(_tags);

            string apiUrl = ("https://api.stackexchange.com/2.3/tags?page=" + _page +
                             "&pagesize=" + _pageSize +
                             "&order=" + _order +
                             "&sort=" + _sort +
                             "&site=" + _site);

            string response  = await http.GetResponse(apiUrl);

            dataProcessing.DecodeJson(response, _pageSize);

            dataProcessing.CalculatePopularity();

            return View(_tags);
        }

    }
}