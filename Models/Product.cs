using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models
{

    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float? weight { get; set; }
        public string Unit { get; set; }
        public string Dimension { get; set; }
        public bool? IsDisabled { get; set; } //for shop ownwr
        public bool? IsAllowed { get; set; } // for Admin
        public virtual ICollection<Image> Images { get; set; }
        public long ClassificationId { get; set; }
         [JsonIgnore]
        public  Classification classification { get; set; }
        // public int Product_Type { get; set; }
        //  [JsonIgnore]
        // public  ProductType ProductType { get; set; }
    
    }

}