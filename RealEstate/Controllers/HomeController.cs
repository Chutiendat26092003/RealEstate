using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Models;
using RealEstate.Models.RealEstateViewModel;
using System.Diagnostics;

namespace RealEstate.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly RealEstateContext _context;


		public HomeController(ILogger<HomeController> logger, RealEstateContext context)
		{
			_logger = logger;
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

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}