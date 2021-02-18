using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet.Models
{
    public class Report
    {
        public string ShopName { get; set; }
        public string ShopOwnerName { get; set; }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }
        public double TotalPayment { get; set; }
        public double CompanyPayment { get; set; }
        public double ShopPayment { get; set; }
        public string AccountNo { get; set; }
    }
}
