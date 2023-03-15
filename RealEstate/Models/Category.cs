using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
	public class Category
	{
		public int Id { get; set; }

		[Required, StringLength(100)]
		public string NameCategory { get; set; }

		public string ImageURL { get; set; }
		public string BannerURL { get; set; }

		public ICollection<Product>? Products { get; set; }
	}
}
