using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;

namespace StoreFlow.ViewComponents
{
    
public class _SalesDataDashboardComponentPartial : ViewComponent
    {
        private readonly StoreContext _context;

        public _SalesDataDashboardComponentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Orders.Include(x => x.customer).Include(y => y.product).OrderByDescending(z =>z.OrderDate).Take(5).ToList();

            return View(values);
        }
    }
}
