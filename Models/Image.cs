using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace dotnet.Models
{
	public class Image
	{
		public long Id { get; set; }
		public string Path { get; set; }
		public long ProductId { get; set; }
         [JsonIgnore]
        public Product Product { get; set; }
	}
}