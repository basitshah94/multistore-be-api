using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
namespace dotnet.Models
{

    public class Transaction
    {
        public long Id { get; set; }
        
        public int Amount {get; set;}
       
        public long UserId { get; set; }
         [JsonIgnore]
        public virtual User User { get; set; }
        
        public long ShopId { get; set; }
         [JsonIgnore]
        public virtual Shop Shop { get; set; }

         public long OrderId { get; set; }
         [JsonIgnore]
        public virtual Order Order { get; set; }

        public DateTime TransactionDate {get; set; }     
    
    }

}