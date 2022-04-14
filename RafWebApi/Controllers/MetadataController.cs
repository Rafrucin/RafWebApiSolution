using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RafWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        //// GET api/<MetadataController>/5
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    List<Metadata> metadata = new List<Metadata>();

        //    var output = Metadata.GetMetadataById(id);

        //    var resp = output.Select(f => new { f.movieId, f.title, f.language, f.duration, f.releaseYear }).ToList();

        //    if (resp.Count==0)
        //    {
        //        return Content(HttpStatusCode.NotFound, "Foo does not exist.");
        //    }

        //    return JsonConvert.SerializeObject(resp);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            List<Metadata> metadata = new List<Metadata>();

            var output = Metadata.GetMetadataById(id);

            var resp = output.Select(f => new { f.movieId, f.title, f.language, f.duration, f.releaseYear }).ToList();

            if (resp.Count == 0)
            {
                return NotFound();
            }

            return Ok(JsonConvert.SerializeObject(resp));
        }

        // POST api/<MetadataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var obj = JsonConvert.DeserializeObject<Metadata>(value);
            var MetadataList = Metadata.CSVreader();
            MetadataList.Add(obj);
            Metadata.CSVWriter(MetadataList);
        }

     
    }


}
