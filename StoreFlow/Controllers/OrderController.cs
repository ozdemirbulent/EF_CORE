using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class OrderController : Controller
    {
        private readonly StoreContext _context;

        public OrderController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult AllCountSmallerThen5()
        {
            bool orderStockCount = _context.Orders.All(x => x.OrderCount <= 5);
            if (orderStockCount)
            {
                ViewBag.message = "Sipariş sayısı 5'ten az olan tüm siparişler mevcut.";
            }
            else
            {
                ViewBag.message = "Sipariş sayısı 5'ten az olan siparişler mevcut değil.";
            }
            return View();
        }

        public IActionResult OrderListByStatus(string status, Order t)
        {
            var values = _context.Orders.Where(t => t.Status != null && t.Status.Contains(status)).Include(t => t.customer).Include(u => u.product).ToList();
            if (!values.Any())
            {
                ViewBag.message = $"{status} durumunda sipariş bulunmamaktadır.";
            }

            return View(values);
        }

        public IActionResult OrderListSearch(string name, string filtertype)
        {
            if (filtertype == "start")
            {
                var values1 = _context.Orders
                    .Where(t => t.Status != null && t.Status.StartsWith(name))
                    .Include(t => t.customer)
                    .Include(u => u.product)
                    .ToList();
                return View(values1);
            }
            else if (filtertype == "end")
            {
                var values1 = _context.Orders
                    .Where(t => t.Status != null && t.Status.EndsWith(name))
                    .Include(t => t.customer)
                    .Include(u => u.product)
                    .ToList();
                return View(values1);
            }
            var values = _context.Orders
                .Where(t => t.Status != null && t.Status.Contains(name))
                .Include(t => t.customer)
                .Include(u => u.product)
                .ToList();
            return View(values);

        }

        public async Task<IActionResult> OrderList()
        {
            var values = await _context.Orders.Include(x => x.product).Include(y => y.customer).ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var products = await _context.Products
              .Select(p => new SelectListItem
              {
                  Value = p.ProductId.ToString(),
                  Text = p.ProductName
              }).ToListAsync();
            ViewBag.Products = products;



            var customers = await _context.Customers
             .Select(c => new SelectListItem
             {
                 Value = c.CustomerId.ToString(),
                 Text = c.CustomerName + " " + c.CustomerSurname
             }).ToListAsync();
            ViewBag.customers = customers;
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> CreateOrder(Order order)
        {
            order.Status = "Sipariş Alındı";
            order.OrderDate = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }


        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("OrderList");
        }

        [HttpGet]

        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            var products = await _context.Products
              .Select(p => new SelectListItem
              {
                  Value = p.ProductId.ToString(),
                  Text = p.ProductName
              }).ToListAsync();
            ViewBag.Products = products;
            var customers = await _context.Customers
             .Select(c => new SelectListItem
             {
                 Value = c.CustomerId.ToString(),
                 Text = c.CustomerName + " " + c.CustomerSurname
             }).ToListAsync();
            ViewBag.customers = customers;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }
        public IActionResult OrderListWithCustomerGroup()
        {
            var result = from customers in _context.Customers
                         join orders in _context.Orders
                         on customers.CustomerId equals orders.CustomerId
                         into OrderGroup
                         select new CustomerOrderViewModel
                         {
                             CustomerName = customers.CustomerName + " " + customers.CustomerSurname,
                             Orders = OrderGroup.ToList()

                         };
            return View(result.ToList());
        }
    }
}
