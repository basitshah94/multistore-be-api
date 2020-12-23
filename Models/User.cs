using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
         [JsonIgnore]
        public virtual Role Role { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public string Email_Address { get; set; }
        public string Password { get; set; }
        public int Code { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsDisabled { get; set; }
        public string Address { get; set; }
        public string Contact_Number { get; set; }
        public string CNIC { get; set; }
        public string CNIC_Image { get; set; }
        public string UserImage{ get; set; }
        public string Site_link { get; set; }
    
    }

}