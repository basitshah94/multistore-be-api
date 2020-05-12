using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models
{

    public class Order
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string BusinessId { get; set; }
        public long UserId { get; set; }
         [JsonIgnore]
        public virtual User User { get; set; }
        public DateTime OrderPlacementDate  { get; set; }
        public DateTime OrderDeliveryDate  { get; set; }
        public long TotalAmmount { get; set; }
         public long? RiderId { get; set; }
         [JsonIgnore]
        public virtual Rider Rider { get; set; }
       public virtual ICollection<OrderItem> OrderItems { get; set; }
    
    }

}