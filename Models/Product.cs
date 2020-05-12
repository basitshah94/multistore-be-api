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
         public long ClassificationId { get; set; }
         [JsonIgnore]
        public  Classification classification { get; set; }
        // public int Product_Type { get; set; }
        //  [JsonIgnore]
        // public  ProductType ProductType { get; set; }
    
    }

}