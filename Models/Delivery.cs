using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace dotnet.Models
{

    public class Delivery
    {
        public long Id { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public long OrderId { get; set; }
        public virtual  Order Order { get; set; }
        public long RiderId { get; set; }
        public virtual  Rider Rider { get; set; }
    
    }

}