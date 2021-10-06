using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagsOnStackOverflow.Models
{
    public class DataProcessing
    {
        private List<Tag> _tags;

        public DataProcessing(List<Tag> tags)
        {
            _tags = tags;
        }

        public void DecodeJson(string response, int _pageSize)
        {
            JObject json = JObject.Parse(response);
            for (int j = 0; j < _pageSize; j++)
            {
                string name = (string)json.SelectToken("items[" + j + "].name");
                int count = (int)json.SelectToken("items[" + j + "].count");
                _tags.Add(new Tag()
                {
                    ID = j + 1,   //1-100
                    Name = name,
                    NumberOfOccurrences = count,
                    Popularity = string.Empty
                }
                );
            }
        }

        public void CalculatePopularity()
        {
            int sum = _tags.Sum(x => x.NumberOfOccurrences);
            _tags.ForEach(x => x.Popularity = string.Format("{0:P3}", (double)x.NumberOfOccurrences / sum));
        }
    }
}