using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using multi_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace multi_store.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _db;

        public UserController(Context context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _db.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetSingle(long id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

         [HttpPost("register")]
          public async Task<ActionResult<User>> Post(User User)
        {
           
            _db.Users.Update(User);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingle), new { id = User.Id }, User);
            
        }

          [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User postedUser)
        {

            var dbUser = await _db.Users.FirstOrDefaultAsync(x=>x.Email_Address==postedUser.Email_Address && x.Password == postedUser.Password );
                 
            if (dbUser == null)
                return NotFound(new { message = "Invalid Email or Password." });

            return dbUser;
        }
        
    }
}