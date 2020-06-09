
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet.Models
{
	public class Classification
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Thumbmail { get; set; }
		public string Banner { get; set; }
		public long CategoryId { get; set; }
        public virtual Category Category { get; set; }		
	}
}