using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using RealEstate.Data;
using RealEstate.Models;
using RealEstate.Models.RealEstateViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace RealEstate.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly RealEstateContext _context;

        public ProductController(IWebHostEnvironment webHostEnvironment, RealEstateContext context)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductAllView();
            viewModel.Products = await _context.Product
                  .Include(m => m.ProductImages)
                  .Include(m => m.TransactionExcepts)
                    .ThenInclude(m => m.User)
                  .AsNoTracking()
                  .OrderBy(m => m.NameProduct)
                  .ToListAsync();
            //Product product = viewModel.Products.Single();
            //viewModel.ProductImages = product.ProductImages;

            //viewModel.User = viewModel.Products.Where();

            viewModel.Categories = await _context.Category.ToListAsync();

            return View(viewModel);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var viewModel = new ProductDetailView();
            viewModel.Product = await _context.Product
                  .Include(m => m.ProductImages)
                  .Include(m => m.TransactionExcepts)
                    .ThenInclude(m => m.User)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(m => m.Id == id);
            Product product = viewModel.Product;
            viewModel.ProductImages = product.ProductImages.ToList();

            viewModel.User = product.TransactionExcepts.Select(p => p.User).Single();

            viewModel.Category = await _context.Category.FindAsync(product.CategoryId);

            return View(viewModel);
        }

        private string UploadedFile(ProductImage productImage)
        {
            string uniqueFileName = null;

            if (productImage.FrontImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FrontImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productImage.FrontImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
