using System;
using System.Collections.Generic;
using System.Text;

namespace SantanderTest.API.Models
{
    public class Story
    {
        public string title { get; set; }
        public string uri { get; set; }
        public string postedBy { get; set; }
        public DateTimeOffset time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
    }
}
