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
    public class ImageController : ControllerBase
    {

          private readonly Context _db;

        public ImageController(Context context)
        {
            _db = context;
        }

        // GET api/image/product/1
        [HttpGet("product/{id}")]
         public async Task<ActionResult<IEnumerable<Image>>> GetAllByProduct(long id)
        {
             var images = await _db.Images.Where(x=>x.ProductId == id).ToListAsync();
            return images;
        }

        // GET api/group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetSingle(long id)
        {
            var image = await _db.Images.FindAsync(id);
            if (image == null)
                return NotFound();

            return image;
        }

        // POST api/group
       [HttpPost]
        public async Task<ActionResult<Image>> Post(Image image)
        {
            _db.Images.Update(image);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = image.Id }, image);
        }

        // PUT api/group/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Image image)
        {
            if (id != image.Id)
                return BadRequest();

            _db.Entry(image).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var image = await _db.Images.FindAsync(id);

            if (image == null)
                return NotFound();

            _db.Images.Remove(image);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
