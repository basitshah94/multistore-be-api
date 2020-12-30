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
    public class ReturnedProductController : ControllerBase
    {

          private readonly Context _db;

        public ReturnedProductController(Context context)
        {
            _db = context;
        }

        // GET api/product
        [HttpGet]
         public async Task<ActionResult<IEnumerable<ReturnedProduct>>> GetAll()
        {
            return await _db.ReturnedProducts.ToListAsync();
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnedProduct>> GetSingle(long id)
        {
            var product = await _db.ReturnedProducts.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            return product;
        }

       

        // POST api/returnedproduct
       [HttpPost]
        public async Task<ActionResult<ReturnedProduct>> Post(ReturnedProduct product)
        {
            _db.ReturnedProducts.Update(product);
            
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = product.Id }, product);
        }

          [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, ReturnedProduct product)
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
            var product = await _db.ReturnedProducts.FindAsync(id);

            if (product == null)
                return NotFound();

            _db.ReturnedProducts.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }

     
    }
}
