using Microsoft.EntityFrameworkCore;

namespace RealEstate.Models.RealEstateViewModel
{
    [Keyless]
    public class ProductDetailView
    {
        public Category? Category { get; set; }
        public Product? Product { get; set; }
        public IEnumerable<ProductImage>? ProductImages { get; set; }
        public User? User { get; set; }
        public TransactionExcept? TransactionExcept { get; set; }

        public List<IFormFile>? files { get; set; }
        public double? TotalTran { get; set; }

    }
}
