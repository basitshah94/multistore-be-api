using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{
   

    public class ReturnedProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Price {get; set;}
    }

}