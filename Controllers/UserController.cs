using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnet.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _db;
        private IConfiguration Configuration;

        public UserController(Context context, IConfiguration _Configuration)
        {
            _db = context;
            Configuration = _Configuration;
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

        [HttpGet("dashboard/byShopowner/data/{id}")]
        public async Task<ActionResult<CustomerDashboard>> GetDashboardDataForOwner(int id)
        {
            var dashboard = new CustomerDashboard();
            var activeShops = _db.Shops.Where(x => x.UserId == id && x.IsVerified == true).Count();
            var deativeShops = _db.Shops.Where(x => x.UserId == id && x.IsVerified == false).Count();
            var productscount = 0;
            var orderCount = 0;

            var Product = await (from Products in _db.Products
                                 join shop in _db.Shops
                                  on Products.ShopId equals shop.Id
                                 where shop.UserId == id
                                 group new { Products, shop } by new { Products.Id } into products
                                 select new CustomerDashboard()
                                 {
                                     Products = products.Count()
                                 }).ToListAsync();

            if (activeShops != 0)
            {
                var activeShopss = _db.Shops.Where(x => x.UserId == id && x.IsVerified == true).Include(x => x.Products).ToList();
                foreach (var shop in activeShopss)
                {
                    productscount += shop.Products.Count();
                }
            }

            var orders = await (from shop in _db.Shops
                                join order in _db.Orders
                                 on shop.Id equals order.ShopId
                                where shop.UserId == id && order.OrderStatus == 0
                                select new Order()
                                {
                                    Id = order.Id
                                }).ToListAsync();

            foreach (var order in orders)
            {
                orderCount += 1;
            }


            dashboard.ActiveShops = activeShops;
            dashboard.DeactiveShops = deativeShops;
            dashboard.Products = productscount;
            dashboard.NewOrders = orderCount;

            return dashboard;
        }

        [HttpGet("dashboard/data")]
        public async Task<ActionResult<Dashboard>> GetDashboardData()
        {
            var dashboard = new Dashboard();
            var Allusers = _db.Users.Count();
            var activeShopOwners = _db.Users.Where(x => x.RoleId == 2 && x.IsDisabled != true).Count();
            var disabledShopOwners = _db.Users.Where(x => x.RoleId == 2 && x.IsDisabled == true).Count();
            var activeCustomers = _db.Users.Where(x => x.RoleId == 3 && x.IsDisabled != true).Count();
            var disabledCustomers = _db.Users.Where(x => x.RoleId == 3 && x.IsDisabled == true).Count();
            var activeRiders = _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled != true).Count();
            var disabledRiders = _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled == true).Count();
            var activeShops = _db.Shops.Where(x => x.IsVerified == true).Count();
            var disabledShops = _db.Shops.Where(x => x.IsVerified != true).Count();
            dashboard.TotalUsers = Allusers;
            dashboard.ActiveShopOwners = activeShopOwners;
            dashboard.DisabledShopOwners = disabledShopOwners;
            dashboard.ActiveCustomers = activeCustomers;
            dashboard.DisabledCustomers = disabledCustomers;
            dashboard.ActiveRiders = activeRiders;
            dashboard.DisabledRiders = disabledRiders;
            dashboard.ActiveShops = activeShops;
            dashboard.PendingShops = disabledShops;

            return dashboard;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Post(User User)
        {
            if (User.RoleId == 4)
                User.IsDisabled = true;
            EmailService email = new EmailService(_db);
            SMSService sms = new SMSService(_db);
            var verifyUser = await _db.Users.Where(x => x.Email_Address == User.Email_Address || x.Contact_Number == User.Contact_Number).FirstOrDefaultAsync();
            if (verifyUser != null)
            {
                if (verifyUser.IsVerified != true)
                {
                    sms.sendCodeSMS(verifyUser.Code, verifyUser.Contact_Number);
                    email.sendCodeEmail(verifyUser.Code, verifyUser.Email_Address);
                    return StatusCode(404, "unverified-" + verifyUser.Id);
                }
                return StatusCode(404, "Email or Mobile Number  Already Exist");
            }
            else
            {
                Random random = new Random();
                User.Code = random.Next(9999);
                User.IsVerified = false;
                _db.Users.Update(User);
                await _db.SaveChangesAsync();
                sms.sendCodeSMS(User.Code, User.Contact_Number);
                var superadmin = await _db.Users.Where(x => x.RoleId == 1).FirstOrDefaultAsync();
                if (User.RoleId == 4)
                    sms.sendRiderAddSMS(superadmin.Contact_Number);  // send sms to admin
                if (User.RoleId == 2)
                    sms.sendShopOwnerAddSMS(superadmin.Contact_Number);
                email.sendCodeEmail(User.Code, User.Email_Address);
                return CreatedAtAction(nameof(GetSingle), new { id = User.Id }, User);
            }

        }

        //   [HttpPost("login")]
        // public async Task<ActionResult<User>> Login(User postedUser)
        // {

        //     var dbUser = await _db.Users.FirstOrDefaultAsync(x=>x.Email_Address==postedUser.Email_Address && x.Password == postedUser.Password );

        //     if (dbUser == null)
        //         return NotFound(new { message = "Invalid Email or Password." });

        //     return dbUser;
        // }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User postedUser)
        {
            var Token = "";
            string[] Roles = new string[] { "role", "SuperAdmin", "ShopOwner", "Customer", "Rider" };
            var dbUser = _db.Users.Where(x => (x.Email_Address == postedUser.Email_Address || x.Contact_Number == postedUser.Contact_Number) && x.Password == postedUser.Password).FirstOrDefault();
            if (dbUser != null)
            {
                if (dbUser.IsVerified != true)
                {
                    EmailService email = new EmailService(_db);
                    SMSService sms = new SMSService(_db);
                    sms.sendCodeSMS(dbUser.Code, dbUser.Contact_Number);
                    email.sendCodeEmail(dbUser.Code, dbUser.Email_Address);
                    return StatusCode(404, "unverified-" + dbUser.Id);
                }

                if (dbUser.IsDisabled == true && dbUser.RoleId != 4)
                {

                    return StatusCode(404, "your account is disabled");

                }

            }
            if (dbUser != null)
            {
                //Token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]));
                var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                    new Claim (JwtRegisteredClaimNames.Sid, dbUser.Id.ToString ()),
                    new Claim ("FirstName", dbUser.FirstName),
                    new Claim ("LastName", dbUser.LastName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim ("RoleName", Roles[dbUser.RoleId]),
                    new Claim ("UserImage", dbUser.UserImage==null || dbUser.UserImage==""?"":dbUser.UserImage),
                    new Claim ("Address", dbUser.Address),
                    new Claim ("Contact", dbUser.Contact_Number),
                    new Claim ("Email", dbUser.Email_Address),
                    new Claim ("IsVerified", dbUser.IsVerified.ToString ())

                    //new Claim(ClaimTypes.Role, dbUser.RoleId.ToString()),

                };
                // if (users.Role.RoleName != null) { new Claim("roles", users.Role.RoleName); }

                var _Token = new JwtSecurityToken(
                    issuer: Configuration["Jwt:Issuer"],
                    audience: Configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: Credentials

                );
                Token = new JwtSecurityTokenHandler().WriteToken(_Token);
                return Ok(new { Token = Token }); ;
            }

            else
                return NotFound(new { message = "Invalid Email or Password." });
        }


        [HttpGet("resendCode/{id}")]
        public async Task<ActionResult<User>> resendCode(long id)
        {
            var dbUser = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            EmailService email = new EmailService(_db);
            SMSService sms = new SMSService(_db);
            sms.sendCodeSMS(dbUser.Code, dbUser.Contact_Number);
            email.sendCodeEmail(dbUser.Code, dbUser.Email_Address);

            return NoContent();
        }

        // [HttpPut("{id}/update-profile")]
        // public async Task<ActionResult<User>> UpdateProfile(long id , User User)
        // {
        //     // if (id != User.Id)
        //     //     return BadRequest();
        //    User dbuser = await _db.Users.FirstOrDefaultAsync(x=>x.Id == id);
        //     dbuser.FirstName = User.FirstName;
        //     dbuser.LastName = User.LastName;
        //     dbuser.Email_Address=User.Email_Address;
        //     dbuser.Contact_Number = User.Contact_Number;
        //     dbuser.Address = User.Address;
        //     dbuser.Site_link = User.Site_link;
        //     _db.Entry(dbuser).State = EntityState.Modified;
        //   // _db.Update(user);
        //     await _db.SaveChangesAsync();

        //     return NoContent();
        // }

        //otp verify
        [HttpGet("Role/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsrsByRole(long id)
        {
            var account = await _db.Users.Where(x => x.RoleId == id).ToListAsync();
            if (account == null)
                return NotFound();

            return account;
        }

        [HttpGet("Rider/disabled")]
        public async Task<ActionResult<IEnumerable<User>>> GetDisabledRiders(long id)
        {
            var account = await _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled == true).ToListAsync();
            if (account == null)
                return NotFound();

            return account;
        }

        [HttpGet("Rider/enabled")]
        public async Task<ActionResult<IEnumerable<User>>> GetEnabledRiders(long id)
        {
            var account = await _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled == false).ToListAsync();
            if (account == null)
                return NotFound();
            return account;
        }

        [HttpPut("Status/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> UpdateUserStatus(long id)
        {
            User dbuser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbuser.IsDisabled == false || dbuser.IsDisabled == null)
            {
                dbuser.IsDisabled = true;
            }
            else
            {
                dbuser.IsDisabled = false;
            }

            _db.Entry(dbuser).State = EntityState.Modified;
            // _db.Update(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("updateimage/{id}/{imgpath}")]
        public string UpdateUserImage(long id, string imgPath)
        {
            User dbuser = _db.Users.FirstOrDefault(x => x.Id == id);
            dbuser.UserImage = imgPath;
            _db.SaveChangesAsync();
            return "success";
        }

        [HttpPost("{id}/verify")]
        public async Task<ActionResult<User>> Verify(long id, [FromBody] int code)
        {
            User dbUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbUser == null)
                return NotFound();

            if (dbUser.Code == code)
            {
                dbUser.IsVerified = true;
                Random random = new Random();
                dbUser.Code = random.Next(9999);
                _db.Users.Update(dbUser);
                await _db.SaveChangesAsync();
                var Token = "";
                string[] Roles = new string[] { "role", "SuperAdmin", "ShopOwner", "Customer", "Rider" };
                //Token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]));
                var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                    new Claim (JwtRegisteredClaimNames.Sid, dbUser.Id.ToString ()),
                    new Claim ("FirstName", dbUser.FirstName),
                    new Claim ("LastName", dbUser.LastName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim ("RoleName", Roles[dbUser.RoleId]),
                    new Claim ("Address", dbUser.Address),
                    new Claim ("Contact", dbUser.Contact_Number),
                    new Claim ("Email", dbUser.Email_Address),
                    new Claim ("IsVerified", dbUser.IsVerified.ToString ())
                };

                var _Token = new JwtSecurityToken(
                    issuer: Configuration["Jwt:Issuer"],
                    audience: Configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: Credentials

                );
                Token = new JwtSecurityTokenHandler().WriteToken(_Token);

                return Ok(new { Token = Token });
            }
            else
            {
                return NotFound(new { message = "Invalid Code." });
            }

        }

        [HttpPut("{id}/update-profile")]
        public async Task<ActionResult<User>> UpdateProfile(long id, User User)
        {
            // if (id != User.Id)
            //     return BadRequest();
            User dbuser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            dbuser.FirstName = User.FirstName;
            dbuser.LastName = User.LastName;
            dbuser.Email_Address = User.Email_Address;
            dbuser.Contact_Number = User.Contact_Number;
            dbuser.Address = User.Address;
            dbuser.UserImage = User.UserImage;
            dbuser.Site_link = User.Site_link;
            _db.Entry(dbuser).State = EntityState.Modified;
            // _db.Update(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/change-password")]
        public async Task<ActionResult<User>> UpdatePassword(long id, User User)
        {
            // if (id != User.Id)
            //     return BadRequest();
            User dbuser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            dbuser.Password = User.Password;
            _db.Entry(dbuser).State = EntityState.Modified;
            // _db.Update(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("forget-password")]
        public async Task<long> ForgetPassword([FromBody] string mobile)
        {
            User dbuser = await _db.Users.FirstOrDefaultAsync(x => x.Contact_Number == mobile || x.Email_Address == mobile);
            EmailService emails = new EmailService(_db);
            emails.sendCodeEmail(dbuser.Code, dbuser.Email_Address);
            SMSService sms = new SMSService(_db);
            sms.sendCodeSMS(dbuser.Code, dbuser.Contact_Number);
            return dbuser.Id;
        }

        [HttpPost("{id}/verifypassword")]
        public async Task<ActionResult<User>> Verifypassword(long id, [FromBody] int code)
        {
            User dbUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbUser.Code == code)
            {
                // return StatusCode (404, "Email Address Already Exist");
                Random random = new Random();
                dbUser.Code = random.Next(9999);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound(new { message = "Invalid Code." });
            }

        }

        public static string sendRequest(string url, string data)
        {
            System.Net.Http.HttpClient c = new System.Net.Http.HttpClient();
            //    Console.WriteLine("***************" + data);
            //string json = JsonConvert.SerializeObject(dicti, Formatting.Indented);
            var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
            // Console.WriteLine("***************"+ httpContent);
            var content = c.PostAsync(url, httpContent);
            return content.ToString();
        }

    }

}