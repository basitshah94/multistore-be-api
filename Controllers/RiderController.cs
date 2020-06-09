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
    public class RiderController : ControllerBase
    {

          private readonly Context _db;

        public RiderController(Context context)
        {
            _db = context;
        }

        // GET api/rider
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Rider>>> GetAll()
        {
            return await _db.Riders.ToListAsync();
        }

        // GET api/rider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rider>> GetSingle(long id)
        {
            var rider = await _db.Riders.FindAsync(id);
            if (rider == null)
                return NotFound();

            return rider;
        }

        // POST api/rider
       [HttpPost]
        public async Task<ActionResult<Rider>> Post(Rider rider)
        {
            _db.Riders.Update(rider);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = rider.Id }, rider);
        }

        // PUT api/rider/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Rider rider)
        {
            if (id != rider.Id)
                return BadRequest();

            _db.Entry(rider).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/rider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var rider = await _db.Riders.FindAsync(id);

            if (rider == null)
                return NotFound();

            _db.Riders.Remove(rider);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
