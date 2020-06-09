using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ProductCode {get; set;}
        public string ProductDetail {get; set;}
        public int? Discount{get; set;}
        public int? Quantity {get; set;}
        public float? Weight { get; set; }
        public string Unit { get; set; }
        public string Dimension { get; set; }
        public bool? IsDisabled { get; set; } //for shop ownwr
        public bool? IsAllowed { get; set; } // for Admin
        public bool? IsOutOfStock {get; set;}
        public bool? IsNew {get; set;}
        public bool? IsSale {get; set;}
        public bool? IsOffer {get; set;}
        public virtual ICollection<Image> Images { get; set; }
        public long ClassificationId { get; set; }
        //  [JsonIgnore]
        public virtual Classification Classification { get; set; }
        public long ShopId { get; set; }
         [JsonIgnore]
        public  Shop Shop { get; set; }
    
    }

}