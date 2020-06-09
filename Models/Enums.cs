using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eatklik.Models
{

    
    public enum OrderStatus
    {
        New,
        Complete,
        Cancel
    }
    
     public enum PaymentMethod
    {
        Cash,
        EasyPaisa,
        JazzCash
    }

     public enum AccountType
    {
        EasyPaisa,
        JazzCash
    }
}