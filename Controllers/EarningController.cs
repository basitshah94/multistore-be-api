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

namespace dotnet.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class EarningController : ControllerBase {
        private readonly Context _db;
        private IConfiguration Configuration;

        public EarningController (Context context, IConfiguration _Configuration) {
            _db = context;
            Configuration = _Configuration;
        }



            [HttpGet ("superadmin/{date}")]  //SHOULD START FROM FIRST OF MONTH
        public async Task<ActionResult<EarningDashboard>> GetErningDashboardSuperAdmin(DateTime date) {
            var dashboard = new EarningDashboard();
            var totalOrders =  _db.Orders.Where(x => x.OrderPlacementDate >= date && x.OrderPlacementDate <= date.AddDays(30) &&  x.OrderStatus == OrderStatus.Complete).ToList();
             var totalOrdersCount =  totalOrders.Count();
           if (totalOrders != null)
           {
                 long totalAmount =  0;
            foreach (var order in totalOrders)
            {
                totalAmount += (order.TotalAmmount - 100);
            }
            var shopAmount = (totalAmount - totalAmount * 0.20);
            var companyAmount = (totalAmount - shopAmount);

            dashboard.TotalOrders = totalOrdersCount;
            dashboard.TotalAmount = totalAmount;
            dashboard.ShopAmount = shopAmount;
            dashboard.CompanyAmount = companyAmount;
             return dashboard;
           }
          else 
          return NotFound();

           
        }

          [HttpGet ("shopowner/{id}/{date}")]  //SHOULD START FROM FIRST OF MONTH
        public async Task<ActionResult<EarningShop>> GetEarningDashboardSuperAdmin(long id , DateTime date) {
            var dashboard = new EarningShop();
            var totalOrders =  _db.Orders.Where(x => x.OrderPlacementDate >= date && x.OrderPlacementDate <= date.AddDays(30) &&  x.OrderStatus == OrderStatus.Complete && x.ShopId == id).ToList();
            var totalOrderCount = totalOrders.Count();
           if (totalOrders != null)
           {
            long totalAmount =  0;
            foreach (var order in totalOrders)
            {
                totalAmount += (order.TotalAmmount - 100);
            }
            var shopAmount = (totalAmount - totalAmount * 0.20);
            var companyAmount = (totalAmount - shopAmount);

            dashboard.TotalOrders = totalOrderCount;
            dashboard.TotalAmount = totalAmount;
            dashboard.ShopAmount = shopAmount;
            dashboard.CompanyAmount = companyAmount;
            dashboard.Orders = totalOrders;
             return dashboard;
           }
          else 
          return NotFound();
       
        }

        [HttpGet ("dashboard/data")]
        public async Task<ActionResult<Dashboard>> GetDashboardData() {
            var dashboard = new Dashboard();
            var Allusers =  _db.Users.Count();
            var activeShopOwners =  _db.Users.Where(x => x.RoleId == 2 &&  x.IsDisabled != true).Count();
            var disabledShopOwners =  _db.Users.Where(x => x.RoleId == 2 &&  x.IsDisabled == true).Count();
            var activeCustomers =  _db.Users.Where(x => x.RoleId == 3 && x.IsDisabled != true).Count();
            var disabledCustomers =  _db.Users.Where(x => x.RoleId == 3 && x.IsDisabled == true).Count();
            var activeRiders =  _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled != true).Count();
            var disabledRiders =  _db.Users.Where(x => x.RoleId == 4 && x.IsDisabled == true).Count();
            var activeShops =  _db.Shops.Where(x => x.IsVerified == true).Count();
            var disabledShops=  _db.Shops.Where(x => x.IsVerified != true).Count();        
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

       

    }

}