using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class Vehicle
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Registration_Number { get; set; }
    
    }

}