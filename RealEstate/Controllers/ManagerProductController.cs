using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Models.RealEstateViewModel;
using RealEstate.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;

namespace RealEstate.Controllers
{
    public class ManagerProductController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly RealEstateContext _context;
        public INotyfService _notyfService { get; }

        public ManagerProductController(IWebHostEnvironment webHostEnvironment, RealEstateContext context, INotyfService notyfService)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
            _notyfService = notyfService;

        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            var accId = User.FindFirstValue("AccountId");
            //var user = _context.User.Where(a => a.Email == email).AsNoTracking().FirstOrDefault();

            var viewModel = new ProductAllView();
            viewModel.User = await _context.User
                  .Include(m => m.TransactionExcepts)
                    .ThenInclude(m => m.Product)
                        .ThenInclude(m => m.ProductImages)
                  .Where(m => m.AccountId == int.Parse(accId))
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            User user = viewModel.User;
            viewModel.Products = user.TransactionExcepts.Select(a => a.Product).OrderByDescending(a => a.Id).Where(a => a.IsDeleted == false ).DistinctBy(a => a.Id);

            // Tạo danh sách sản phẩm phân trang
            var products = viewModel.Products.ToList().AsQueryable();
            var totalProducts = viewModel.Products.Count();
            ViewBag.totalPage = (int)Math.Ceiling((double)totalProducts / pageSize);
            var pagedProducts = new PagedList<Product>(products, pageNumber, pageSize);
            //var pagedProducts = new StaticPagedList<Product>(products, pageNumber, pageSize, totalPage);
            viewModel.PagedProducts = pagedProducts;

            CheckTimeTrnasaction();

            return View(viewModel);
        }
        public async void CheckTimeTrnasaction()
        {
            var transactionExcepts = await _context.TransactionExcept.Where(a => a.EndTime == false).AsNoTracking().ToListAsync();
            if (transactionExcepts != null)
            {
                foreach (var item in transactionExcepts)
                {
                    var seconds_Time = (item.Time - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                    var seconds_Day = item.Day * 24 * 60 * 60;
                    var secondsCurrent = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                    bool isActive = seconds_Time + seconds_Day > secondsCurrent;

                    if (!isActive)
                    {
                        item.EndTime = true;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }

                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();
            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "Id", "NameCategory");

            var viewModel = new ProductDetailView();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDetailView productDetailView)
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();
            var userId = ViewBag.user.Id;
            var user = ViewBag.user;

            var productReq = productDetailView.Product;
            var transactionExceptReq = productDetailView.TransactionExcept;
            var totalTran = productDetailView.TotalTran;

            if (user.Balance >= totalTran)
            {
                user.Balance -= totalTran;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                _notyfService.Error("Số dư không đủ mời bạn nạp thêm!");
                return View(productDetailView);
            }

            var product = new Product
            {
                NameProduct = productReq.NameProduct,
                Price = productReq.Price,
                AddressProduct = productReq.AddressProduct,
                Directory = productReq.Directory,
                Area = productReq.Area,
                Bedroom = productReq.Bedroom,
                Restrooms = productReq.Restrooms,
                HomeOrientation = productReq.HomeOrientation,
                Description = productReq.Description,
                IsDeleted = false,
                CategoryId = 1,
            };
            _context.Add(product);
            await _context.SaveChangesAsync();

            //Save image to wwwroot/image
            foreach (IFormFile img in productDetailView.files)
            {

                //Save image to wwwroot/image
                string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                string extension = Path.GetExtension(img.FileName);
                string imgURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine("wwwroot/img", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await img.CopyToAsync(fileStream);
                }


                var productImage = new ProductImage
                {
                    NameImage = imgURL,
                    ProductId = product.Id,
                };
                _context.Add(productImage);
                await _context.SaveChangesAsync();

            }


            var transaction = new Transaction
            {
                Status = '1',
                Amount = Convert.ToDouble(totalTran),
                UserId = userId,
            };
            _context.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionExcept = new TransactionExcept
            {
                Time = DateTime.Now,
                Day = transactionExceptReq.Day,
                ServicePackage = transactionExceptReq.ServicePackage,
                EndTime = false,
                TransactionId = transaction.Id,
                UserId = userId,
                ProductId = product.Id,
            };
            _context.Add(transactionExcept);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();
            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "Id", "NameCategory");

            var viewModel = new ProductDetailView();
            viewModel.Product = await _context.Product
                  .Include(m => m.ProductImages)
                  .Include(m => m.TransactionExcepts)
                    .ThenInclude(m => m.User)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(m => m.Id == id);
            Product product = viewModel.Product;
            viewModel.ProductImages = product.ProductImages.ToList();

            //List<SelectListItem> categories = ViewBag.Categories;
            foreach (var category in ViewBag.Categories)
            {
                if (category.Value == product.CategoryId.ToString())
                {
                    category.Selected = true;
                    break;
                }
            }

            viewModel.User = product.TransactionExcepts.Select(p => p.User).Single();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Renewal(int id)
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();

            var viewModel = new ProductDetailView();
            viewModel.Product = await _context.Product
                  .Include(m => m.ProductImages)
                  .Include(m => m.TransactionExcepts)
                    .ThenInclude(m => m.User)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(m => m.Id == id);
            Product product = viewModel.Product;
            viewModel.TransactionExcept = product.TransactionExcepts.LastOrDefault();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Renewal(ProductDetailView productDetailView)
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();
            var userId = ViewBag.user.Id;
            var user = ViewBag.user;

            var transactionExceptReq = productDetailView.TransactionExcept;
            var totalTran = productDetailView.TotalTran;

            if (user.Balance >= totalTran)
            {
                user.Balance -= totalTran;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                _notyfService.Error("Số dư không đủ mời bạn nạp thêm!");
                return View(productDetailView);
            }

            var transaction = new Transaction
            {
                Status = '1',
                Amount = Convert.ToDouble(totalTran),
                UserId = userId,
            };
            _context.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionExcept = new TransactionExcept
            {
                Time = DateTime.Now,
                Day = transactionExceptReq.Day,
                ServicePackage = transactionExceptReq.ServicePackage,
                TransactionId = transaction.Id,
                UserId = userId,
                ProductId = productDetailView.Product.Id,
            };
            _context.Add(transactionExcept);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            var accId = User.FindFirstValue("AccountId");
            ViewBag.user = _context.User.Where(a => a.AccountId == int.Parse(accId)).AsNoTracking().FirstOrDefault();

            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ContosoUniversityContext.Student'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                _context.Update(product);
                //_context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

