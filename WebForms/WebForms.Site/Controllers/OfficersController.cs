using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebForms.Site.Models;

namespace WebForms.Site.Controllers
{
    public class OfficersController : ApiController
    {
        private readonly OfficerServiceContext context;

        public OfficersController() : this(new OfficerServiceContext())
        {
        }

        public OfficersController(OfficerServiceContext context)
        {
            this.context = context;
        }

        // GET api/<controller>
        public IEnumerable<Officer> Get() => context.Officers;

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            var officer = context.Officers.Find(id);
            if (officer == null) return NotFound();

            return Ok(officer);
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> PostAsync([FromBody] Officer officer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                context.Officers.Add(officer);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = officer.SerialNo }, officer);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(string id, [FromBody] Officer officer)
        {
            return BadRequest();
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> DeleteAsync(string id)
        {
            try
            {
                var entity = context.Officers.Find(id);
                context.Officers.Remove(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}