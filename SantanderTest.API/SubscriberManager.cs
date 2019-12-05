using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using SantanderTest.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantanderTest.API
{
    public class SubscriberManager
    {
        IMemoryCache _cache;
        public SubscriberManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Run()
        {
            Task.Run(() =>
            {
                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Options.ReceiveHighWatermark = 1000;
                    subSocket.Connect("tcp://localhost:12345");
                    subSocket.Subscribe("Stories");
                    Console.WriteLine("Subscriber socket connecting...");
                    while (true)
                    {
                        string topicReceived = subSocket.ReceiveFrameString();
                        string storiesMsg = subSocket.ReceiveFrameString();
                        var stories = JsonConvert.DeserializeObject<List<Story>>(storiesMsg);

                        Console.WriteLine($"Received {stories.Count} {topicReceived}");

                        _cache.Set(topicReceived, stories);
                    }
                }
            });
        }
    }
}
