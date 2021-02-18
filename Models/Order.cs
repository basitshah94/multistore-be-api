using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{
     public enum OrderStatus
    {
        New,
        Assigned,
        Complete,
        Cancel
    }

     public enum PaymentMethod
    {
        EasyPaisa,
        JazzCash
    }

    public class Order
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime OrderPlacementDate  { get; set; }
        public DateTime OrderDeliveryDate  { get; set; }
        public string DelieveryAddress { get; set; }
        public long TotalAmmount { get; set; }
        public PaymentMethod PaymentMethod {get ; set;}   //0 for cash //1 for easypaisa // 2 for  jazzcash
        public OrderStatus OrderStatus {get ; set;} 
        public double CustomerLat {get; set;}
        public double CustomerLong {get; set;}
        public int OrderCode {get; set;}
        public bool? IsReceived {get; set;}
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public long ShopId { get; set; }
        public virtual Shop Shop {get; set;}
        public long? RiderId { get; set; }
        public virtual User Rider { get; set; }
        public long? ReturnedProductId { get; set; }
         public int? ReturnQuantity { get; set; }
         public int? deliveryCharges { get; set; }
         public long? ReturnDiscount { get; set; }
        public virtual ReturnedProduct ReturnedProduct { get; set; }
       public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

}