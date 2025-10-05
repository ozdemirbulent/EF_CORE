using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;
using StoreFlow.Models;

namespace StoreFlow.ViewComponents
{
    public class _DailySalesDashboardComponentPartial : ViewComponent
    {
        private readonly StoreContext _context;
        public _DailySalesDashboardComponentPartial(StoreContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = _context.todo
                .GroupBy(t => t.Priority)
                .Select(g => new ToDoStatusChartViewModel
                {
                    status = g.Key,
                    count = g.Count()
                }).ToList();
            return View(data);
        }
    }
}