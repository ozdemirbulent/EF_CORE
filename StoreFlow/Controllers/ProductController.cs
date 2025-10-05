using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreContext _context;

        public ProductController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult ProductList()
        {
            var values = _context.Products.Include(y => y.category).Take(10).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            var categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var categories = _context.Categories
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryId.ToString(),
                   Text = c.CategoryName
               }).ToList();

            ViewBag.Categories = categories;

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("ProductList");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }


        public IActionResult FirstProductList()
        {
            var values = _context.Products.Include(x => x.category).Take(5).ToList();
            return View(values);
        }

        public IActionResult SkipFourProductList()
        {
            var values = _context.Products.Include(x => x.category).Skip(5).Take(5).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProductWithAttach()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CreateProductWithAttach(Product product)
        {
            var category = new Category { CategoryId = 1 };
            _context.Categories.Attach(category);

            var productValue = new Product
            {
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductStock = product.ProductStock,
                CategoryId = category.CategoryId,
                category = category,
                orders = new List<Order>() 
            };

            _context.Products.Add(productValue);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult ProductCount()
        {
            var values = _context.Products.LongCount();
            var result = _context.Products.OrderBy(x => x.ProductId).Last();
            var first = _context.Products.OrderBy(x => x.ProductId).First();
            var firstProduct = _context.Products.OrderBy(x => x.ProductId).Select(y => y.ProductPrice).First();
            var lastProduct = _context.Products.OrderBy(x => x.ProductId).Select(y => y.ProductPrice).Last();
            var firstProductStock = _context.Products.OrderBy(x => x.ProductId).Select(y => y.ProductStock).First();
            var lastProductStock = _context.Products.OrderBy(x => x.ProductId).Select(y => y.ProductStock).Last();
            var xproduct = _context.Products.Include(x => x.category).OrderBy(x => x.ProductId).Select(y => y.category).First();
            var yproduct = _context.Products.Include(x => x.category).OrderBy(x => x.ProductId).Select(y => y.category).Last();
            ViewBag.y = yproduct.CategoryName;
            ViewBag.x = xproduct.CategoryName;
            ViewBag.ls = lastProductStock;
            ViewBag.fs = firstProductStock;
            ViewBag.l = lastProduct;
            ViewBag.c = firstProduct;
            ViewBag.f = first.ProductName;
            ViewBag.v = result.ProductName;
            ViewBag.value = values;
            return View();

        }
        public IActionResult CategoryWithByProducts()
        {
            var values = from c in _context.Categories
                         join p in _context.Products
                         on c.CategoryId equals p.CategoryId
                         select new ProductWithCategoryViewModel
                         {
                             ProductName = p.ProductName,
                             ProductStock = p.ProductStock,
                             CategoryName = c.CategoryName ?? string.Empty 
                         };

            return View(values.ToList());
        }

    }
}
