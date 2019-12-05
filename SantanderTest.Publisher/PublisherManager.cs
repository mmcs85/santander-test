using SantanderTest.Publisher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NetMQ.Sockets;
using NetMQ;
using Newtonsoft.Json;

namespace SantanderTest.Publisher
{
    public class PublisherManager
    {
        private static HttpHandler HttpHandler = new HttpHandler();
        public int IntervalSecs { get; set; }
        public int NrStories { get; set; }

        public PublisherManager(int intervalSecs, int nrStories)
        {
            this.IntervalSecs = intervalSecs;
            this.NrStories = nrStories;
        }

        public async Task<List<Story>> FetchBestStories()
        {
            var bestStories = await HttpHandler.GetJsonAsync<List<ulong>>("https://hacker-news.firebaseio.com/v0/beststories.json");
            List<Task<HNStory>> storiesTasks = new List<Task<HNStory>>();
            List<Story> result = null;

            try
            {
                foreach (var storyId in bestStories.Take(NrStories))
                {
                    storiesTasks.Add(HttpHandler.GetJsonAsync<HNStory>($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json"));
                }

                HNStory[] hnStories = await Task.WhenAll(storiesTasks);

                result = hnStories.AsEnumerable()
                     .Select(n => new Story() { 
                        title = n.title,
                        uri = n.url,
                        postedBy = n.by,
                        score = n.score,
                        time = new DateTime(n.time),
                        commentCount = n.kids != null ? n.kids.Count : 0
                    }).OrderByDescending((Story n) => n.score)
                    .ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ex: {ex.Message}");
            }

            return result;
        }

        public void Run()
        {
            Task.Run(async () =>
            {
                using (var pubSocket = new PublisherSocket())
                {
                    Console.WriteLine("Publisher socket binding...");
                    pubSocket.Options.SendHighWatermark = 1000;
                    pubSocket.Bind("tcp://localhost:12345");

                    while (true)
                    {
                        var stories = await FetchBestStories();

                        if(stories != null)
                        {
                            Console.WriteLine($"Sending {stories.Count} stories");
                            pubSocket.SendMoreFrame("Stories").SendFrame(JsonConvert.SerializeObject(stories));
                        }
                        
                        await Task.Delay(IntervalSecs * 1000);
                    }
                }
            });
        }
    }
}
