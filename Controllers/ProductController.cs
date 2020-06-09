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
    public class ProductController : ControllerBase
    {

          private readonly Context _db;

        public ProductController(Context context)
        {
            _db = context;
        }

        // GET api/product
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _db.Products.Include(x=>x.Images).Include(x=>x.Classification).ThenInclude(x=>x.Category).ThenInclude(x=>x.Group).ToListAsync();
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetSingle(long id)
        {
            var product = await _db.Products.Include(x=>x.Images).Include(x=>x.Classification).ThenInclude(x=>x.Category).ThenInclude(x=>x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            return product;
        }

        [HttpGet("shop/{id}")]
         public async Task<ActionResult<IEnumerable<Product>>> GetAll(long id)
        {
            return await _db.Products.Include(x=>x.Images).Include(x=>x.Classification).ThenInclude(x=>x.Category).ThenInclude(x=>x.Group).Where(x=>x.ShopId == id).ToListAsync();
        }

        // POST api/product
       [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            _db.Products.Update(product);
            
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = product.Id }, product);
        }

        // PUT api/product/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _db.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
