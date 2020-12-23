using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class CustomerDashboard
    {
        public int ActiveShops { get; set; }
        public int DeactiveShops { get; set; }
        public int Products { get; set; }
    
    }

}