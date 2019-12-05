using System;
using System.Collections.Generic;
using System.Text;

namespace SantanderTest.Publisher.Models
{
    public class HNStory
    {
        public string title { get; set; }
        public string url { get; set; }
        public string by { get; set; }
        public long time { get; set; }
        public int score { get; set; }
        public List<ulong> kids { get; set; }
    }
}
