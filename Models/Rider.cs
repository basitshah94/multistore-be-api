using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models
{

    public class Rider
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DeliveryArea { get; set; }
        public string CNIC { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
         public long VehicleId { get; set; }
         [JsonIgnore]
        public virtual Vehicle Vehicle { get; set; }
    
    }

}