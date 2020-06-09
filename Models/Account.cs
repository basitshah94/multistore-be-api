using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{

    public class Account
    {
        public long Id { get; set; }
        public long UserId { get; set; }
         [JsonIgnore]
        public virtual User User { get; set; }
        public string MSISDN {get; set; }
        public bool? IsVerified { get; set; }
        public AccountType AccountType { get; set; }  //0 for easypaisa //1 for jazzcash 
        public string CNIC {get; set;}
    }

     public enum AccountType
    {
        EasyPaisa,
        JazzCash
    }

}