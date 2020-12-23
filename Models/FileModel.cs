using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class FileModel
    {
        public string Classification { get; set; }
        public string Description { get; set; }
        public string Dimension { get; set; }
        public int Discount { get; set; }
        public string[] Images { get; set; }
        public bool IsAllowed { get; set; }
        public bool Isdisable { get; set; }
        public bool IsOffer { get; set; }
        public bool IsNew { get; set; }
        public bool IsoutofStock { get; set; }
        public bool IsSale { get; set; }
        public int Price { get; set; }
        public string ProductCode { get; set; }
        public int  Quantity { get; set; }
        public string ShopName { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public float Weight { get; set; }
    }
}