using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace dotnet.Models
{
	public class Group
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Thumbnail { get; set; }
		public string Banner { get; set; }
		public virtual ICollection<Category> Categories { get; set; }
	}
}