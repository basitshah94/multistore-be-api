using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace dotnet.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly Context _db;
        private IConfiguration Configuration;

        public UserController (Context context, IConfiguration _Configuration) {
            _db = context;
            Configuration = _Configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll () {
            return await _db.Users.ToListAsync ();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<User>> GetSingle (long id) {
            var user = await _db.Users.FindAsync (id);
            if (user == null)
                return NotFound ();

            return user;
        }

        [HttpPost ("register")]
        public async Task<ActionResult<User>> Post (User User) {
            Random random = new Random ();
            User.Code = random.Next (9999);
            User.IsVerified = false;
            _db.Users.Update (User);
            await _db.SaveChangesAsync ();
            sendemail(User.Code , User.Email_Address);
            return CreatedAtAction (nameof (GetSingle), new { id = User.Id }, User);

        }
          // send email function 
        public static void sendemail (int code , string useremail) {
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage ("multistore199@gmail.com", useremail)) {
                mm.Subject = "multistore account verification";
                string body = "Your Account verification code is  " + code ;
                mm.Body = body;
                mm.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                // SmtpClient smtp = new SmtpClient ();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                // System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("multistore199@gmail.com", "Love@Pakistan@123");
                NetworkCredential NetworkCred = new NetworkCredential ("multistore199@gmail.com", "Love@Pakistan@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send (mm);

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
        [HttpPost ("login")]
        public async Task<ActionResult<User>> Login (User postedUser) {
            var Token = "";
            string[] Roles = new string[] { "role", "SuperAdmin", "ShopOwner", "Customer", "ServiceProvider" };
            var dbUser = await _db.Users.FirstOrDefaultAsync (x => x.Email_Address == postedUser.Email_Address && x.Password == postedUser.Password);

            if (dbUser != null) {
                //Token
                var securityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["Jwt:key"]));
                var Credentials = new SigningCredentials (securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new [] {
                    new Claim (JwtRegisteredClaimNames.Sid, dbUser.Id.ToString ()),
                    new Claim ("FirstName", dbUser.FirstName),
                    new Claim ("LastName", dbUser.LastName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim ("RoleName", Roles[dbUser.RoleId]),
                    new Claim ("Address", dbUser.Address),
                    new Claim ("Contact", dbUser.Contact_Number),
                    new Claim ("Email", dbUser.Email_Address),
                    new Claim ("IsVerified", dbUser.IsVerified.ToString ())

                    //new Claim(ClaimTypes.Role, dbUser.RoleId.ToString()),

                };
                // if (users.Role.RoleName != null) { new Claim("roles", users.Role.RoleName); }

                var _Token = new JwtSecurityToken (
                    issuer: Configuration["Jwt:Issuer"],
                    audience : Configuration["Jwt:Issuer"],
                    claims,
                    expires : DateTime.Now.AddMinutes (120),
                    signingCredentials : Credentials

                );
                Token = new JwtSecurityTokenHandler ().WriteToken (_Token);

            }

            if (dbUser == null)
                return NotFound (new { message = "Invalid Email or Password." });

            return Ok (new { Token = Token });;
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

        [HttpPost ("{id}/verify")]
        public async Task<ActionResult<User>> Verify (long id , [FromBody] int code) {
             User dbUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        
            if (dbUser == null)
            return NotFound();
           
            if (dbUser.Code == code) {
                dbUser.IsVerified = true;
                Random random = new Random ();
                dbUser.Code = random.Next (9999);
                _db.Users.Update (dbUser);
                await _db.SaveChangesAsync ();
                var Token = "";
                string[] Roles = new string[] { "role", "SuperAdmin", "ShopOwner", "Customer", "ServiceProvider" };
                //Token
                var securityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["Jwt:key"]));
                var Credentials = new SigningCredentials (securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new [] {
                    new Claim (JwtRegisteredClaimNames.Sid, dbUser.Id.ToString ()),
                    new Claim ("FirstName", dbUser.FirstName),
                    new Claim ("LastName", dbUser.LastName),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                    new Claim ("RoleName", Roles[dbUser.RoleId]),
                    new Claim ("Address", dbUser.Address),
                    new Claim ("Contact", dbUser.Contact_Number),
                    new Claim ("Email", dbUser.Email_Address),
                    new Claim ("IsVerified", dbUser.IsVerified.ToString ())

                    //new Claim(ClaimTypes.Role, dbUser.RoleId.ToString()),

                };
                // if (users.Role.RoleName != null) { new Claim("roles", users.Role.RoleName); }

                var _Token = new JwtSecurityToken (
                    issuer : Configuration["Jwt:Issuer"],
                    audience : Configuration["Jwt:Issuer"],
                    claims,
                    expires : DateTime.Now.AddMinutes (120),
                    signingCredentials : Credentials

                );
                Token = new JwtSecurityTokenHandler ().WriteToken (_Token);

                return Ok (new { Token = Token });
            } else {
                return NotFound (new { message = "Invalid Code." });
            }

        }

        [HttpPut ("{id}/update-profile")]
        public async Task<ActionResult<User>> UpdateProfile (long id, User User) {
            // if (id != User.Id)
            //     return BadRequest();
            User dbuser = await _db.Users.FirstOrDefaultAsync (x => x.Id == id);
            dbuser.FirstName = User.FirstName;
            dbuser.LastName = User.LastName;
            dbuser.Email_Address = User.Email_Address;
            dbuser.Contact_Number = User.Contact_Number;
            dbuser.Address = User.Address;
            dbuser.Site_link = User.Site_link;
            _db.Entry (dbuser).State = EntityState.Modified;
            // _db.Update(user);
            await _db.SaveChangesAsync ();

            return NoContent ();
        }

        [HttpPut ("{id}/change-password")]
        public async Task<ActionResult<User>> UpdatePassword (long id, User User) {
            // if (id != User.Id)
            //     return BadRequest();
            User dbuser = await _db.Users.FirstOrDefaultAsync (x => x.Id == id);
            dbuser.Password = User.Password;
            _db.Entry (dbuser).State = EntityState.Modified;
            // _db.Update(user);
            await _db.SaveChangesAsync ();

            return NoContent ();
        }

      

    }
    
}