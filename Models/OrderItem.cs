using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class OrderItem
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public virtual Product Product {get;set;}
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
        public long OrderId {get; set;}
        [JsonIgnore]
        public virtual Order Order {get;set;}
    }

}