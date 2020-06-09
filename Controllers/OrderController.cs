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
  
       [HttpGet("customer/{id}")]
         public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer(long id)
        {
            var orders =  await _db.Orders.Where(x=>x.UserId == id).ToListAsync();
            return orders;
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

            foreach (var item in order.OrderItems)
            {
              Product product = await _db.Products.Where(x=>x.Id == item.ProductId).FirstOrDefaultAsync();
              product.Quantity = product.Quantity - item.Quantity;
               _db.SaveChangesAsync();
            }

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
