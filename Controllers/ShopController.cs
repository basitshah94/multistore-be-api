using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase {

        private readonly Context _db;

        public ShopController (Context context) {
            _db = context;
        }

        // GET api/group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetAll () {
            return await _db.Shops.Include (x => x.Group).ToListAsync ();
        }

        [HttpGet ("{lat}/{lng}")]
        public async Task<ActionResult<ICollection<Shop>>> GetInField (float lat, float lng) {
            var shops = await _db.Shops.Include (x => x.Group).Where(x=>x.IsVerified == true || x.IsDisabled==false).ToListAsync ();
           // List<Shop> fieldShops = new List<Shop> ();
            foreach (var shop in shops) {
                var distance = CalculateDistance (lat, lng, shop.Latitude, shop.Longitude);
                if (distance <= shop.DeliveryRadius) {
                    //fieldShops.Add (shop);
                    shop.IsInRange = true;
                }
            }
            return shops;
        }

        [HttpGet ("{lat}/{lng}/{groupid}")]
        public async Task<ActionResult<ICollection<Shop>>> GetByCategory (float lat, float lng, long groupid) {
            var shops = await _db.Shops.Include (x => x.Group).Where(x=>x.IsVerified == true && x.GroupId == groupid).ToListAsync ();
            foreach (var shop in shops) {
                var distance = CalculateDistance (lat, lng, shop.Latitude, shop.Longitude);
                if (distance <= shop.DeliveryRadius) {
                    shop.IsInRange = true;
                }
            }
            return shops;
        }

        [HttpGet ("user/{id}")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetByUser (long id) {
            return await _db.Shops.Where (x => x.UserId == id).Include (x => x.Group).ToListAsync ();
        }

        [HttpGet ("verified")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetVerifiedShops () {
            return await _db.Shops.Where (x => x.IsVerified == true).Include (x => x.Group).ToListAsync ();
        }

        [HttpGet ("unverfied")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetUnVerified () {
            return await _db.Shops.Where (x => x.IsVerified == false).Include (x => x.Group).ToListAsync ();
        }

        // GET api/group/5
        [HttpGet ("{id}")]
        public async Task<ActionResult<Shop>> GetSingle (long id) {
            var shop = await _db.Shops.Where(x=>x.Id == id).Include(x=>x.User).FirstOrDefaultAsync();
            if (shop == null)
                return NotFound ();

            return shop;
        }

        [HttpGet ("{id}/products")]
        public async Task<ActionResult<Shop>> GetWithProducts (long id) {
            return await _db.Shops.Include (x => x.Products).ThenInclude (x => x.Images).Where (x => x.Id == id).FirstOrDefaultAsync ();
        }

        // POST api/group
        [HttpPost]
        public async Task<ActionResult<Shop>> Post (Shop shop) {
            _db.Shops.Update (shop);
            await _db.SaveChangesAsync ();
            var superadmin = await _db.Users.Where (x => x.RoleId == 1).FirstOrDefaultAsync ();
            EmailService email = new EmailService (_db);
             SMSService sms = new SMSService (_db);
            email.sendShopEmail (shop.UserId, superadmin.Email_Address);
              sms.sendShopAddSMS(superadmin.Contact_Number);
            return CreatedAtAction (nameof (GetSingle), new { id = shop.Id }, shop);
        }

       

        // PUT api/group/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put (long id, Shop shop) {
            if (id != shop.Id)
                return BadRequest ();

            _db.Entry (shop).State = EntityState.Modified;
            await _db.SaveChangesAsync ();

            return NoContent ();
        }

        // DELETE api/group/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (long id) {
            var shop = await _db.Shops.FindAsync (id);

            if (shop == null)
                return NotFound ();

            _db.Shops.Remove (shop);
            await _db.SaveChangesAsync ();

            return NoContent ();
        }

        public static double CalculateDistance (double sLatitude, double sLongitude, double eLatitude,
            double eLongitude) {
            var radiansOverDegrees = (Math.PI / 180.0);
            var sLatitudeRadians = sLatitude * radiansOverDegrees;
            var sLongitudeRadians = sLongitude * radiansOverDegrees;
            var eLatitudeRadians = eLatitude * radiansOverDegrees;
            var eLongitudeRadians = eLongitude * radiansOverDegrees;

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow (Math.Sin (dLatitude / 2.0), 2.0) +
                Math.Cos (sLatitudeRadians) * Math.Cos (eLatitudeRadians) *
                Math.Pow (Math.Sin (dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                Math.Atan2 (Math.Sqrt (result1), Math.Sqrt (1.0 - result1));

            return result2;
        }
    }
}