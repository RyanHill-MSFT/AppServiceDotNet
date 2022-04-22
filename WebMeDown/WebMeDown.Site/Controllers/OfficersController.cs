using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMeDown.Site.Models;

namespace WebMeDown.Site.Controllers
{
    public class OfficersController : ApiController
    {
        private readonly OfficerContext officerContext;

        public OfficersController()
        {
            officerContext = new OfficerContext();
        }

        // GET api/<controller>
        public IEnumerable<Officer> Get() => officerContext.Officers;

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            var officer = officerContext.Officers.Find(id);
            if (officer == null) return NotFound();

            return Ok(officer);
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
        }
    }
}