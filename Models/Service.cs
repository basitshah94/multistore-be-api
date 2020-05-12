using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models
{

    public class Service
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Charges { get; set; }
    
    }

}