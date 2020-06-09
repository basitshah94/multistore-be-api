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
    public class CategoryController : ControllerBase
    {

          private readonly Context _db;

        public CategoryController(Context context)
        {
            _db = context;
        }

        // GET api/category
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await _db.Categories.ToListAsync();
        }

        // GET api/category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetSingle(long id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return category;
        }

        // POST api/category
       [HttpPost]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = category.Id }, category);
        }

        // PUT api/category/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            _db.Entry(category).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
