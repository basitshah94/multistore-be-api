using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class EarningDashboard
    {
        public int TotalOrders { get; set; }
        public long TotalAmount { get; set; }
        public double ShopAmount { get; set; }
        public double CompanyAmount { get; set; }
        public int Orders { get; set; }
    
    }

}