using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string NameProduct { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập")]
        public string AddressProduct { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập")]
        public char Directory { get; set; } //  cần bán - cho thuê

        [Required(ErrorMessage = "Vui lòng nhập")]
        public double Area { get; set; } // diện tích đất

        [Required(ErrorMessage = "Vui lòng nhập")]
        public int Bedroom { get; set; } // phòng ngủ

        [Required(ErrorMessage = "Vui lòng nhập")]
        public int Restrooms { get; set; } // nhà vs

        [Required(ErrorMessage = "Vui lòng nhập")]
        public string HomeOrientation { get; set; } // hướng nhà

        [Required(ErrorMessage = "Vui lòng nhập")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //public ICollection<ProductDetail>? ProductDetails { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<TransactionExcept>? TransactionExcepts { get; set; }
    }
}
