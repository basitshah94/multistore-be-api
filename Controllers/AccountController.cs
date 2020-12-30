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
    public class AccountController : ControllerBase
    {

          private readonly Context _db;

        public AccountController(Context context)
        {
            _db = context;
        }

        // GET api/group
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            return await _db.Accounts.ToListAsync();
           // return await _db.Groups.ToListAsync();
        }

        // GET api/group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetSingle(long id)
        {
            var account = await _db.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return account;
        }

         [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetUserAccounts(long id)
        {
            var account = await _db.Accounts.Where(x=>x.UserId == id).ToListAsync();
            if (account == null)
                return NotFound();

            return account;
        }
        

        // POST api/group
       [HttpPost]
        public async Task<ActionResult<Account>> Post(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingle), new { id = account.Id }, account);
        }

        // PUT api/group/5
       [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Account account)
        {
            if (id != account.Id)
                return BadRequest();

            _db.Entry(account).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var account = await _db.Accounts.FindAsync(id);

            if (account == null)
                return NotFound();

            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
