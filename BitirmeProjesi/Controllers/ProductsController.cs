using Business.Models;
using Business.Services.Bases;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BitirmeProjesi.Models;
using BitirmeProjesi.Settings;

namespace BitirmeProjesi.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public ProductsController(IProductService productService, ICategoryService categoryService, IStoreService storeService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _storeService = storeService;
        }

        public IActionResult Index(ProductsIndexViewModel viewModel)
        {
            //var result = _productService.List(viewModel.Filter, viewModel.PageNumber, AppSettings.RecordsPerPage, viewModel.Expression, viewModel.IsDirectionAscending);
            //viewModel.Products = result.Data;
            //viewModel.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            //viewModel.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            //viewModel.TotalRecordCount = _productService.GetTotalRecordsCount(viewModel.Filter);
            //var pages = _productService.GetPages(viewModel.TotalRecordCount, AppSettings.RecordsPerPage);
            //viewModel.Pages = new SelectList(pages, viewModel.PageNumber);
            //viewModel.SortBy = new SelectList(_productService.GetExpressions());
            //return View(viewModel);

            return View(_productService.Query().ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", "Id is required");
            }

            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id.Value);
            if (product == null)
            {
                return View("Error", "Product not found!");
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            ProductModel model = new ProductModel()
            {
                ExpirationDate = DateTime.Today,
                UnitPrice = 0,
                StockQuantity = 0
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (UpdateImage(product, image) == false)
                {
                    ModelState.AddModelError("", $"File could not be uploaded: Extensions should be {AppSettings.AcceptedImageExtensions}, and file size should be maximum {AppSettings.AcceptedImageExtensions} MB!");
                }
                else
                {
                    var result = _productService.Add(product);
                    if (result.IsSuccessful)
                    {
                        TempData["Message"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", result.Message);
                }
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id);
            if (product == null)
                return View("Error", "Product not found!");
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
			return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (UpdateImage(product, image) == false)
                {
                    ModelState.AddModelError("", $"File could not be uploaded: Extensions should be {AppSettings.AcceptedImageExtensions}, and file size should be maximum {AppSettings.AcceptedImageExtensions} MB!");
                }
                else
                {
                    var result = _productService.Update(product);
                    if (result.IsSuccessful)
                        return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", result.Message);
                }
            }
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");


			return View(product);
        }

        private bool? UpdateImage(ProductModel model, IFormFile uploadedImage)
        {
            bool? result = null;
            string uploadedFileName = null, uploadedFileExtension = null;
            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                result = false;
                uploadedFileName = uploadedImage.FileName;
                uploadedFileExtension = Path.GetExtension(uploadedFileName);
                string[] imageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                foreach (string imageExtension in imageExtensions)
                {
                    if (uploadedFileExtension.ToLower() == imageExtension.ToLower().Trim())
                    {
                        result = true;
                        break;
                    }
                }
                if (result == true)
                {
                    double imageFileSize = AppSettings.MaxAcceptedImageSize * Math.Pow(1024, 2);
                    if (uploadedImage.Length <= imageFileSize)
                        result = true;
                }
            }

            if (result == true)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    uploadedImage.CopyTo(memoryStream);
                    model.Image = memoryStream.ToArray();
                    model.ImageExtension = uploadedFileExtension;
                }
            }

            return result;
        }

        public IActionResult Delete(int? id)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
                return RedirectToAction("UnauthorizedAction", "Accounts");
            if (id == null)
                return View("Error", "Id is required!");
            var result = _productService.Delete(id.Value);
            if (result.IsSuccessful)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteImage(int productId)
        {
            _productService.DeleteImage(productId);
            return RedirectToAction(nameof(Details), new { id = productId });
        }
    }
}