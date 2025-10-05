using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using System.Linq;

namespace StoreFlow.Controllers
{
    public class CategoryController : Controller
    {
        private readonly StoreContext _context;

        public CategoryController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult CategoryList()
        {
            var values = _context.Categories.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateCategory(Category category)
        {

            category.CategoryStatus = false;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryList");

        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var category = _context.Categories.Find(id);
            return View(category);


        }
        [HttpPost]


        public IActionResult UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public IActionResult ReverseCategory()
        {
            var category = _context.Categories.First();
            ViewBag.b = category.CategoryName;

            var singlecategories = _context.Categories.SingleOrDefault(y => y.CategoryName == "Giyim - Erkek");
            ViewBag.c = singlecategories != null
                ? singlecategories.CategoryStatus + " - " + singlecategories.CategoryId
                : "Category not found";
            var values = _context.Categories.OrderBy(x => x.CategoryId).Reverse().ToList();
            return View(values);
        }

    }
}