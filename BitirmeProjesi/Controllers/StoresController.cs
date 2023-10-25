using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoresController : Controller
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: Stores
        public ActionResult Index()
        {
            var model = _storeService.Query().ToList();
            return View(model);
        }

        // GET: Stores/Details/5
        public ActionResult Details(int id)
        {
            var model = _storeService.Query().SingleOrDefault(m => m.Id == id);
            return View(model);
        }

        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _storeService.Add(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _storeService.Query().SingleOrDefault(m => m.Id == id);
            return View(model);
        }

        // POST: Stores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoreModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _storeService.Update(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return View("Error", "Id is required!");
            Result result = _storeService.Delete(id.Value);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
