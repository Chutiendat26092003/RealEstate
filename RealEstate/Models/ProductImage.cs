using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public string NameImage { get; set; }

        [Required(ErrorMessage = "Please choose Front image")]
        [NotMapped]
        public IFormFile FrontImage { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
