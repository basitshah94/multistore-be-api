using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using multi_store.Models;
using Microsoft.EntityFrameworkCore;

namespace multi_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {

          private readonly Context _db;

        public ShopController(Context context)
        {
            _db = context;
        }

        // GET api/group
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Shop>>> GetAll()
        {
            return await _db.Shops.ToListAsync();
        }

        // GET api/group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetSingle(long id)
        {
            var shop = await _db.Shops.FindAsync(id);
            if (shop == null)
                return NotFound();

            return shop;
        }

        // POST api/group
       [HttpPost]
        public async Task<ActionResult<Shop>> Post(Shop shop)
        {
            _db.Shops.Update(shop);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = shop.Id }, shop);
        }

        // PUT api/group/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Shop shop)
        {
            if (id != shop.Id)
                return BadRequest();

            _db.Entry(shop).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var shop = await _db.Shops.FindAsync(id);

            if (shop == null)
                return NotFound();

            _db.Shops.Remove(shop);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
