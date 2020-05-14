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
    public class OrderController : ControllerBase
    {

          private readonly Context _db;

        public OrderController(Context context)
        {
            _db = context;
        }

        // GET api/order
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            return await _db.Orders.ToListAsync();
        }

        // GET api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetSingle(long id)
        {
            var order = await _db.Orders.Include(x=>x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                return NotFound();

            return order;
        }

        // POST api/order
       [HttpPost]
        public async Task<ActionResult<Order>> Post(Order order)
        {
            _db.Orders.Update(order);
            
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = order.Id }, order);
        }

        // PUT api/order/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            _db.Entry(order).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
