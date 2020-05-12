using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models
{

    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
         [JsonIgnore]
        public virtual Role Role { get; set; }
        public string NDN_Number { get; set; }
        public string Business_Name { get; set; }
        public string Email_Address { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Contact_Number { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Site_link { get; set; }
    
    }

}