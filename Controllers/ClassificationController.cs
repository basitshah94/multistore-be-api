using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {

          private readonly Context _db;

        public ClassificationController(Context context)
        {
            _db = context;
        }

        // GET api/classification
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Classification>>> GetAll()
        {
            return await _db.Classifications.ToListAsync();
        }

        // GET api/Classification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classification>> GetSingle(long id)
        {
            var classification = await _db.Classifications.FindAsync(id);
            if (classification == null)
                return NotFound();

            return classification;
        }

        // POST api/classification
       [HttpPost]
        public async Task<ActionResult<Classification>> Post(Classification classification)
        {
            _db.Classifications.Update(classification);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = classification.Id }, classification);
        }

        // PUT api/classification/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Classification classification)
        {
            if (id != classification.Id)
                return BadRequest();

            _db.Entry(classification).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/classification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var classification = await _db.Classifications.FindAsync(id);

            if (classification == null)
                return NotFound();

            _db.Classifications.Remove(classification);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
