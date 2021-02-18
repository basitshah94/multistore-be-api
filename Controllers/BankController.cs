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
    public class BankController : ControllerBase
    {

          private readonly Context _db;

        public BankController(Context context)
        {
            _db = context;
        }

        // GET api/bank
        [HttpGet]
         public async Task<ActionResult<IEnumerable<Bank>>> GetAll()
        {
            return await _db.Bank.ToListAsync();
        }

      
    }
}
