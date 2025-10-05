using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;

namespace StoreFlow.ViewComponents
{
    public class _Last5ProductsDashboardComponentPartial : ViewComponent

    {
       public StoreContext _context;

        public _Last5ProductsDashboardComponentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Products.OrderBy(x => x.ProductId).ToList().SkipLast(8).ToList().TakeLast(5).ToList();
            return View(values);

        }
    }
}
