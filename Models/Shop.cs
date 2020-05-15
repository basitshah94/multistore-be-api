using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace multi_store.Models

{
public class Shop
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Address { get; set; }
	public long UserId { get; set; }
	[JsonIgnore]
    public User User { get; set; }
	public string Contact { get; set; }
	public string Logo { get; set; }
	public string NDN_Number { get; set; }
	public string Latitude { get; set; }
    public string Longitude { get; set; }
	public string OwnerCNiC {get; set;}
	public bool? IsVerified { get; set; }
	public bool? IsDisabled { get; set; }
	public virtual ICollection<Product> Products { get; set; }
	
}	

}