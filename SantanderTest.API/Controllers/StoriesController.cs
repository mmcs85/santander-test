using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SantanderTest.API.Models;

namespace SantanderTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private IMemoryCache _cache;

        public StoriesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        // GET api/stories
        [HttpGet]
        public ActionResult<List<Story>> Get()
        {
            if(_cache.TryGetValue("Stories", out List<Story> stories))
            {
                return stories;
            }

            return new List<Story>();
        }
    }
}
