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
    public class OrderController : ControllerBase {

        private readonly Context _db;

        public OrderController (Context context) {
            _db = context;
        }

        // GET api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll () {
           return await _db.Orders.Include (x => x.Shop).Include (x => x.User).ToListAsync ();       
        }

        [HttpGet ("customer/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer (long id) {
            var orders = await _db.Orders.Where (x => x.UserId == id).ToListAsync ();
            return orders;
        }

        [HttpGet ("{lat}/{lng}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetNearOrders (float lat , float lng) 
        {
            List<Order> fieldOrders = new List<Order> ();
            var orders = await _db.Orders.Where(x=>x.OrderStatus == 0).ToListAsync ();
             foreach (var order in orders) {
                var distance = CalculateDistance (lat, lng, order.CustomerLat, order.CustomerLong);
                if (distance <= 10) {
                    fieldOrders.Add (order);
                }
            }
            return fieldOrders;
        }

        [HttpGet ("rider/{riderId}/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetRiderAssignedOrders (long riderId , OrderStatus status) 
        {
            var orders = await _db.Orders.Where(x=>x.OrderStatus == status && x.RiderId == riderId).ToListAsync();
            return orders;
        }

        [HttpGet ("usershop/{id}/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByUserShops (long id , OrderStatus status) {
        var orders = await (from shop in _db.Shops 
        join order in _db.Orders
         on shop.Id equals order.ShopId
         where shop.UserId == id && order.OrderStatus == status
         select new Order() {
            Id =order.Id,
            Description=order.Description,
            OrderPlacementDate=order.OrderPlacementDate,
            OrderDeliveryDate=order.OrderDeliveryDate,
            TotalAmmount=order.TotalAmmount,
            PaymentMethod=order.PaymentMethod,
            OrderStatus=order.OrderStatus
        }).ToListAsync();
        return orders;
    }

    // GET api/order/5
    [HttpGet ("{id}")]
    public async Task<ActionResult<Order>> GetSingle (long id) {
        var order = await _db.Orders.Include(x => x.Shop).Include(x=>x.User).Include(x=>x.Rider).Include (x => x.OrderItems).ThenInclude (x => x.Product).FirstOrDefaultAsync (x => x.Id == id);
        if (order == null)
            return NotFound ();

        return order;
    }

    [HttpGet ("{id}/orderitems")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems (long id) {
        var orderitems = await _db.OrderItem.Where (x => x.OrderId == id).Include (x => x.Product).ToListAsync ();
        if (orderitems == null)
            return NotFound ();

        return orderitems;
    }

    // POST api/order
    [HttpPost]
    public async Task<ActionResult<Order>> Post (Order order) {
       Random random = new Random ();
         order.OrderCode = random.Next (999999);
         order.OrderPlacementDate = DateTime.UtcNow.AddHours(5);
          order.OrderDeliveryDate = DateTime.UtcNow.AddHours(5);
          _db.Orders.Update (order);

         await _db.SaveChangesAsync ();

        foreach (var item in order.OrderItems) {
            Product product = await _db.Products.Where (x => x.Id == item.ProductId).FirstOrDefaultAsync ();
            product.Quantity = product.Quantity - item.Quantity;
            _db.SaveChangesAsync ();
        }
var User = await _db.Users.Where(x=> x.Id == order.UserId).FirstOrDefaultAsync();
        EmailService email = new EmailService (_db);
            SMSService sms = new SMSService (_db);
                sms.sendOrderCodeSMS (order.OrderCode, User.Contact_Number);
                email.sendOrderCode (order.OrderCode, User.Email_Address);

            var Riders = await _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled==false && x.IsVerified==true).ToListAsync();
            foreach (var rider in Riders) {
                var Message = "New Order is placed. login to kuicksave for details";
                sms.sendMessage(Message, rider.Contact_Number);
            }
        return CreatedAtAction (nameof (GetSingle), new { id = order.Id }, order);
    }

    // PUT api/order/5
    [HttpPut ("{id}")]
    public async Task<IActionResult> Put (long id, Order order) {
        if (id != order.Id)
            return BadRequest ();

        _db.Entry (order).State = EntityState.Modified;
        await _db.SaveChangesAsync ();

        return NoContent ();
    }
   
    [HttpPut ("{id}/{riderid}/{status}")]
    public async Task<IActionResult> updateStatus (long id, long riderid , OrderStatus status) {
        User rider = await _db.Users.Where(x=>x.Id == riderid).FirstOrDefaultAsync();
        if (rider.IsDisabled == true)
        return StatusCode (404, "you are not approved");
        var order = await _db.Orders.Where(x=>x.Id == id).FirstOrDefaultAsync();
        if (order == null)
            return BadRequest ();
        if (status == OrderStatus.Assigned)
        {
            var checkOrder = await _db.Orders.Where(x=>x.RiderId == riderid && x.OrderStatus == status).FirstOrDefaultAsync();
            if (checkOrder != null)
            return StatusCode (404, "you can accept only one order at a time");
        }
        order.RiderId = riderid;
        order.OrderStatus = status;
        await _db.SaveChangesAsync ();
        return Ok();
    }

    [HttpPut ("complete/{id}/{orderCode}/{status}")]
    public async Task<IActionResult> CompleteOrder (long id, int orderCode , OrderStatus status) {
        var order = await _db.Orders.Where(x=>x.Id == id).FirstOrDefaultAsync();
        if (order == null)
            return BadRequest ();
        if (order.OrderCode == orderCode)
        {
            order.OrderStatus = status;
            order.OrderDeliveryDate = DateTime.UtcNow.AddHours(5);
        await _db.SaveChangesAsync ();
        return Ok();
        }
        else 
        return StatusCode (404, "invalid Order code");
        
    }




    [HttpPut ("{id}/changestatus/{status}")]
    public async Task<IActionResult> updateStatus (long id, OrderStatus status) {
        var order = await _db.Orders.Where(x=>x.Id == id).FirstOrDefaultAsync();
        if (order == null)
            return BadRequest ();
        order.OrderStatus = status;
        await _db.SaveChangesAsync ();
        return Ok();
    }

     [HttpPut ("{id}/markreceived")]
    public async Task<IActionResult> updateStatus (long id) {
        var order = await _db.Orders.Where(x=>x.Id == id).FirstOrDefaultAsync();
        if (order == null)
            return BadRequest ();
        order.IsReceived = true;
        await _db.SaveChangesAsync ();
        return Ok();
    }

    // DELETE api/order/5
    [HttpDelete ("{id}")]
    public async Task<IActionResult> Delete (long id) {
        var order = await _db.Orders.FindAsync (id);

        if (order == null)
            return NotFound ();

        _db.Orders.Remove (order);
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
        [HttpGet("rider/{Id}")]
        public async Task<ActionResult<RiderDashboard>> GetRiderDashboard(int Id)
        {
            var dashboard = new RiderDashboard();
            var PendingOrders = _db.Orders.Where(x => x.OrderStatus == OrderStatus.Assigned && x.RiderId == Id).Count();
            var CompleteOrders = _db.Orders.Where(x => x.OrderStatus == OrderStatus.Complete && x.RiderId == Id).Count();
            var TotalOrders = PendingOrders + CompleteOrders;

            dashboard.PendingOrder = PendingOrders;
            dashboard.CompleteOrder = CompleteOrders;
            dashboard.TotalOrders = TotalOrders;

            return dashboard;

        }
    }
}