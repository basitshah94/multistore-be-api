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
    public class VehicleController : ControllerBase
    {

          private readonly Context _db;

        public VehicleController(Context context)
        {
            _db = context;
        }

        // GET api/vehicle
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Vehicle>>> GetAll()
        {
            return await _db.Vehicles.ToListAsync();
        }

        // GET api/vehicle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetSingle(long id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            return vehicle;
        }

        // POST api/values
       [HttpPost]
        public async Task<ActionResult<Vehicle>> Post(Vehicle vehicle)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = vehicle.Id }, vehicle);
        }

        // PUT api/vehicle/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
                return BadRequest();

            _db.Entry(vehicle).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound();

            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
