using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class Dashboard
    {
        public int TotalUsers { get; set; }
        public int ActiveShopOwners { get; set; }
        public int DisabledShopOwners { get; set; }
        public int ActiveCustomers { get; set; }
        public int DisabledCustomers { get; set; }
        public int ActiveRiders { get; set; }
        public int DisabledRiders { get; set; }
        public int ActiveShops { get; set; }
        public int PendingShops { get; set; }
    
    }

}