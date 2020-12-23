using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class EarningShop
    {
        public int TotalOrders { get; set; }
        public double TotalAmount { get; set; }
        public double ShopAmount { get; set; }
        public double CompanyAmount { get; set; }
        public virtual ICollection<Order> Orders {get; set;}
    
    }

}