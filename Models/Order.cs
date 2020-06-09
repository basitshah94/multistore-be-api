using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{
     public enum OrderStatus
    {
        New,
        Complete,
        Cancel
    }

     public enum PaymentMethod
    {
        Cash,
        EasyPaisa,
        JazzCash
    }

    public class Order
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime OrderPlacementDate  { get; set; }
        public DateTime OrderDeliveryDate  { get; set; }
        public long TotalAmmount { get; set; }
        public bool IsSelfPick {get; set;}
        public bool? IsPaymentDone {get; set;}
        public string RiderName {get; set;}
        public string RiderVehicleNo {get; set;}
        public string RiderContactNo {get; set;}
        public PaymentMethod PaymentMethod {get ; set;}   //0 for cash //1 for easypaisa // 2 for  jazzcash
        public OrderStatus OrderStatus {get ; set;} 
       
        public long UserId { get; set; }
         [JsonIgnore]
        public User User { get; set; }
       
        public long ShopId { get; set; }
        [JsonIgnore]
        public Shop Shop {get; set;}
       
         public long? RiderId { get; set; }
         [JsonIgnore]
        public virtual Rider Rider { get; set; }
       
       public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

}