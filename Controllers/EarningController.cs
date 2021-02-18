using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dotnet.Controllers
{

    [Route ("api/[controller]")]
    [ApiController]
    public class EarningController : ControllerBase {
        private readonly Context _db;
        private IConfiguration Configuration;

        public EarningController (Context context, IConfiguration _Configuration) {
            _db = context;
            Configuration = _Configuration;
        }



            [HttpGet ("superadmin/{datefrom}/{dateto}")]  //SHOULD START FROM FIRST OF MONTH
        public async Task<ActionResult<EarningDashboard>> GetErningDashboardSuperAdmin(DateTime datefrom, DateTime dateto) {
            var dashboard = new EarningDashboard();
            var totalOrders =  _db.Orders.Where(x => x.OrderPlacementDate.Date >= datefrom.Date && x.OrderPlacementDate.Date <= dateto.Date &&  x.OrderStatus == OrderStatus.Complete).ToList();
             var totalOrdersCount =  totalOrders.Count();
           if (totalOrders != null)
           {
                 long totalAmount =  0;
            foreach (var order in totalOrders)
            {
                totalAmount += (order.TotalAmmount);
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
            var totalOrders =  _db.Orders.Include(x=>x.Shop).ThenInclude(x=>x.User).Where(x => x.OrderPlacementDate.Date >= date.Date && x.OrderPlacementDate.Date <= date.AddDays(30).Date &&  x.OrderStatus == OrderStatus.Complete && x.ShopId == id).ToList();
            var totalOrderCount = totalOrders.Count();
           if (totalOrders != null)
           {
            long totalAmount =  0;
            foreach (var order in totalOrders)
            {
                totalAmount += (order.TotalAmmount);
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
        [HttpGet ("downloadcsv/{datefrom}/{dateto}")] 
        public IActionResult GenerateReport (DateTime datefrom,DateTime dateto) {

            var ReportList = new List<Report>();

            var ShopOwners = _db.Users.Include("Shops").Where(x=>x.RoleId==2 && x.IsDisabled !=true && x.IsVerified==true).ToList();
            foreach (var Shopowner in ShopOwners) {
                foreach (var shop in Shopowner.Shops) {
                    var totalOrders = _db.Orders.Where(x => x.OrderPlacementDate.Date >= datefrom.Date && x.OrderPlacementDate.Date <= dateto.Date && x.OrderStatus == OrderStatus.Complete && x.ShopId == shop.Id).ToList();
                    var totalOrderCount = totalOrders.Count();
                    if (totalOrders != null)
                    {
                        long totalAmount = 0;
                        foreach (var order in totalOrders)
                        {
                            totalAmount += (order.TotalAmmount);
                        }
                        var shopAmount = (totalAmount - totalAmount * 0.20);
                        var companyAmount = (totalAmount - shopAmount);
                        ReportList.Add(new Report
                        {
                            ReportFrom = datefrom,
                            ReportTo = dateto,
                            ShopOwnerName = Shopowner.FirstName + ' ' + Shopowner.LastName,
                            ShopName = shop.Name,
                            AccountNo = shop.AccountNumber + "(" + (shop.AccountType=="Bank"?shop.BankName:shop.AccountType)+ ")",
                            CompanyPayment = companyAmount,
                            ShopPayment = shopAmount,
                            TotalPayment = totalAmount
                        }) ; 
                    }
                }
            }

            var stream = new MemoryStream ();
            using (var writeFile = new StreamWriter (stream, Encoding.UTF8, 512, true)) {
                var csv = new CsvWriter (writeFile, CultureInfo.InvariantCulture);
                csv.WriteRecords(ReportList);
            }
            stream.Position = 0; //reset stream
            return File (stream, "application/octet-stream", "Report From Date "+datefrom+" To Date "+dateto+".csv");
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