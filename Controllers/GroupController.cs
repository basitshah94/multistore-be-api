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
    public class GroupController : ControllerBase
    {

          private readonly Context _db;

        public GroupController(Context context)
        {
            _db = context;
        }

        // GET api/group
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Group>>> GetAll()
        {
            return await _db.Groups.Include(x=>x.Categories).ThenInclude(x=>x.Classifications).ToListAsync();
           // return await _db.Groups.ToListAsync();
        }

        // GET api/group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetSingle(long id)
        {
            var group = await _db.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            return group;
        }

        // POST api/group
       [HttpPost]
        public async Task<ActionResult<Group>> Post(Group group)
        {
            _db.Groups.Update(group);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = group.Id }, group);
        }

        // PUT api/group/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Group group)
        {
            if (id != group.Id)
                return BadRequest();

            _db.Entry(group).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var group = await _db.Groups.FindAsync(id);

            if (group == null)
                return NotFound();

            _db.Groups.Remove(group);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
