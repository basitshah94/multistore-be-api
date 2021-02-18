using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models

{
public class Shop
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Address { get; set; }
	public long UserId { get; set; }
    public virtual User User { get; set; }
	public long GroupId{get; set; }
	public long DeliveryRadius {get; set;}
    public virtual Group Group { get; set; }
	public string Contact { get; set; }
	public string Logo { get; set; }
	public string Banner { get; set; }
	public string NDN_Number { get; set; }
	public double Latitude { get; set; }
    public double Longitude { get; set; }
	public bool? IsInRange { get; set; }
	public bool? IsVerified { get; set; }
	public bool? IsDisabled { get; set; }
	public bool? IsReturnable { get; set; }
	public string AccountType { get; set; }
	public string AccountNumber { get; set; }
	public string BankName { get; set; }
	public virtual ICollection<Product> Products { get; set; }
	
}	

}