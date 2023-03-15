using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace RealEstate.Models.RealEstateViewModel
{
    [Keyless]
    public class ProductAllView
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<ProductImage>? ProductImages { get; set; }
        public User? User { get; set; }
        public IEnumerable<TransactionExcept>? TransactionExcepts { get; set; }
        public IPagedList<Product>? PagedProducts { get; set; }

    }
}
