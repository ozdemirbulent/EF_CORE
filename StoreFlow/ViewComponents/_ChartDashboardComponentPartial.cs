using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;
using StoreFlow.Models;

namespace StoreFlow.ViewComponents
{
    public class _ChartDashboardComponentPartial : ViewComponent
    {
        private readonly StoreContext _Context;
        public _ChartDashboardComponentPartial(StoreContext context)
        {
            _Context = context;
        }

        public IViewComponentResult Invoke()
        {
           return View();
        }
    }
}
