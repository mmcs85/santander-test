using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using SantanderTest.Publisher.Models;
using System.Threading.Tasks;

namespace SantanderTest.Publisher
{
    class Program
    {      
        static void Main(string[] args)
        {
            Console.WriteLine("Creating publisher");

            var publisher = new PublisherManager(5, 20);
            publisher.Run();

            Console.ReadLine();
            Console.WriteLine("Closing publisher");
        }
    }
}
