using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace dotnet.Models
{
	public class Category
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Thumnail { get; set; }
		public string Banner { get; set; }
		public long GroupId { get; set; }
        public virtual Group Group { get; set; }	
		public virtual ICollection<Classification> Classifications { get; set; }	
	}
}