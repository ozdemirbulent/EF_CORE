using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;


namespace StoreFlow.ViewComponents.StatisticsViewComponents
{
    public class _StatisticsWidgetComponentPartial : ViewComponent
    {
        private readonly StoreContext _context;

        public _StatisticsWidgetComponentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.categoryCount = _context.Categories.Count();
            ViewBag.ProductMaxPrice = _context.Products.Max(x => x.ProductPrice) + " TL";
            ViewBag.ProductNameMax = _context.Products.OrderByDescending(x => x.ProductPrice).Select(x => x.ProductName).FirstOrDefault();

            ViewBag.ProductMinPrice = _context.Products.Min(x => x.ProductPrice) + " TL";
            ViewBag.productNameMin = _context.Products.OrderBy(x => x.ProductPrice).Select(x => x.ProductName).FirstOrDefault();
            ViewBag.totalSumProductStock = _context.Products.Sum(x => x.ProductStock);

            ViewBag.averageProductStock = _context.Products.Average(x => x.ProductStock).ToString("F2");
            ViewBag.averageProductPrice = _context.Products.Average(x => x.ProductPrice).ToString("F2") + " TL";
            ViewBag.totalProductCount = _context.Products.Where(x => x.ProductPrice > 100).Count();

            ViewBag.totalProductStockName = _context.Products.OrderBy(x => x.ProductStock).Select(y => y.ProductName).FirstOrDefault();
            ViewBag.totalProductId = _context.Products.Min(x => x.ProductStock);

            ViewBag.totalMaxProductStockName = _context.Products.OrderByDescending(x => x.ProductStock).Select(y => y.ProductName).FirstOrDefault();
            ViewBag.totalMaxProductId = _context.Products.Max(x => x.ProductStock);

            ViewBag.categoryProductStock = _context.Categories.Where(x => x.CategoryName == "Elektronik").Select(y => y.Products.Sum(z => z.ProductStock)).FirstOrDefault();
            ViewBag.ProductCountBetween50And100 = _context.Products.Where(x => x.ProductStock < 100 && x.ProductStock > 50).Count();


            ViewBag.product4ıdName = _context.Products.Where(x => x.ProductId == 4).Select(y => y.ProductName).FirstOrDefault();
            return View();
        }
    }
}
