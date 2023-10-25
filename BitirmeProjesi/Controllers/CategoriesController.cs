using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<CategoryModel> model = _categoryService.Query().ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateGet()
        {
            return View("CreateHtml");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePost(string Name, string Description)
        {
            CategoryModel model = new CategoryModel()
            {
                Name = Name,
                Description = Description
            };
            var result = _categoryService.Add(model);
            if (result.IsSuccessful)
                return RedirectToAction(nameof(Index));
            return View("Error", result.Message);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            CategoryModel model = _categoryService.Query().SingleOrDefault(k => k.Id == id);
            if (model == null)
                return View("Error", "Category not found!");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            CategoryModel model = _categoryService.Query().SingleOrDefault(k => k.Id == id.Value);
            if (model == null)
                return View("Error", "Category not found!");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            Result result = _categoryService.Delete(id.Value);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public ContentResult GetHtmlContent()
        {
            return Content("<b><i>Content result.</i></b>", "text/html");
        }

        public ActionResult GetCategoriesXmlContent()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xml += "<CategoryModels>";
            xml += "<CategoryModel>";
            xml += "<Id>" + 1 + "</Id>";
            xml += "<Name>" + "Laptop" + "</Name>";
            xml += "<Description>" + "Laptops" + "</Description>";
            xml += "</CategoryModel>";
            xml += "</CategoryModels>";
            return Content(xml, "application/xml");
        }

        public string GetString()
        {
            return "String.";
        }

        public EmptyResult GetEmpty()
        {
            return new EmptyResult();
        }
    }
}
