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
    public class TransactionController : ControllerBase
    {

          private readonly Context _db;

        public TransactionController(Context context)
        {
            _db = context;
        }

        // GET api/group
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        {
            return await _db.Transactions.ToListAsync();
           
        }

        // GET api/group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetSingle(long id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            return transaction;
        }


       // GET api/group
        [HttpGet("shop/{id}")]
         public async Task<ActionResult<IEnumerable<Transaction>>> GetShopTransactions(long id)
        {
            return await _db.Transactions.Where(x=>x.ShopId == id).ToListAsync();
           
        }
        // POST api/group
       [HttpPost]
        public async Task<ActionResult<Transaction>> Post(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = transaction.Id }, transaction);
        }

        // PUT api/group/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest();

            _db.Entry(transaction).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var transaction = await _db.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound();

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
