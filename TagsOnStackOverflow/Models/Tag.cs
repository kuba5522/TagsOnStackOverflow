using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagsOnStackOverflow.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberOfOccurrences { get; set; }
        public string Popularity { get; set; }
    }
}