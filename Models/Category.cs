using Newtonsoft.Json;

namespace multi_store.Models
{
	public class Category
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Thumnail { get; set; }
		public string Banner { get; set; }
		public long GroupId { get; set; }
         [JsonIgnore]
        public  Group group { get; set; }	
	}
}