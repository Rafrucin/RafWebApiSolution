using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RafWebApi.Controllers
{
    [Route("[controller]/stats")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // GET: api/<MoviesController>
        [HttpGet]
        public string Get()
        {
            var avg = Stats.StatsAvg();
            var kk = Metadata.GetMetadata4Stats();

            var joinBoth = kk.Join(avg, kkrr => kkrr.movieId, avgrr => avgrr.movieId,
                (kkrr, avgrr) => new MovieStat
                {
                    movieId = kkrr.movieId,
                    title = kkrr.title,
                    averageWatchDurationS = avgrr.averageWatchDurationS,
                    watches = avgrr.watches,
                    releaseYear = kkrr.releaseYear

                });
            var ending = joinBoth.OrderByDescending(o => o.watches).ThenByDescending(p => p.releaseYear).ToList();

            return JsonConvert.SerializeObject(ending);

        }      
    }
}
