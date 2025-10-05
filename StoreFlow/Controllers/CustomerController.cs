using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class CustomerController : Controller
    {
        private readonly StoreContext _context;

        public CustomerController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult CustomerList()
        {
            var values = _context.Customers.ToList();
            return View(values);
        }

        public IActionResult CustomerListOrderByCustomerName()
        {
            var values = _context.Customers.OrderBy(x => x.CustomerName).ThenBy(y => y.CustomerSurname).ToList();
            return View(values);
        }

        public IActionResult CustomerListOrderByDescending()
        {
            var values = _context.Customers.OrderByDescending(x => x.CustomerBalance).ToList();
            return View(values);
        }

        public IActionResult CustomerGetByCity(string city)
        {
            var exist = _context.Customers.Any(x => x.CustomerCity == city);
            if (exist)
            {
                ViewBag.message = $" {city} şehrinde Müşteri bulundu.";
            }
            else
            {
                ViewBag.message = $" {city} şehrinde Müşteri yok.";
            }
            return View();
        }


        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateCustomer(Customer customer)
        {


            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");

        }

        public IActionResult DeleteCustomer(int id)
        {
            var Customer = _context.Customers.Find(id);
            if (Customer != null)
            {
                _context.Customers.Remove(Customer);
                _context.SaveChanges();
            }
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }


        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer)
        {

            _context.Customers.Update(customer);
            _context.SaveChanges();

            TempData["Success"] = "Müşteri başarıyla güncellendi!";
            return RedirectToAction("CustomerList");
        }

        public IActionResult CustomerListByCity()
        {
            var groupCustomers = _context.Customers
                .ToList()
                .GroupBy(c => c.CustomerCity)
                .ToList();
            return View(groupCustomers);
        }

        public IActionResult CustomersByCityCount()
        {
            var query =
                from c in _context.Customers
                group c by c.CustomerCity into cityGroup
                select new CustomerCityGroup
                {
                    City = cityGroup.Key,
                    CustomerCount = cityGroup.Count()
                };
            var model = query.OrderByDescending(x => x.CustomerCount).ToList();
            return View(model);

        }


        public IActionResult CustomerCityList()
        {
            var values = _context.Customers.Select(x => x.CustomerCity).Distinct().ToList();
            return View(values);
        }

        public IActionResult ParallelCustomers()
        {
            var customers = _context.Customers.ToList();
            var customerNames = customers
                .AsParallel()
                .Where(c => !string.IsNullOrEmpty(c.CustomerName) && c.CustomerName.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                .ToList();
            return View(customerNames);
        }

        public IActionResult CustomerListExceptCityIstanbul()
        {
            var customers = _context.Customers.ToList();

            var filteredCustomers = _context.Customers
                .Where(x => x.CustomerCity == "İstanbul")
                .ToList();

            var result = customers.ExceptBy(filteredCustomers.Select(f => f.CustomerId), c => c.CustomerId)
                                  .ToList();

            return View(result);
        }

        public IActionResult CustomerListWithDefaultIfEmpty()
        {
            var customers = _context.Customers.ToList();
            var result = customers.Where(c => c.CustomerCity == "Adana")
                                  .DefaultIfEmpty(new Customer
                                  {
                                      CustomerId = 0,
                                      CustomerName = "Müşteri Bulunamadı",
                                      CustomerSurname = "------",
                                      CustomerCity = "",
                                      CustomerBalance = 0
                                  })
                                  .ToList();
            return View(result);
        }
        public IActionResult CustomerIntersectByCity()
        {
            var cityvalues1 = _context.Customers.Where(x => x.CustomerCity == "İstanbul")
                .Select(y => y.CustomerName + " " + y.CustomerSurname)
                .ToList();
            var cityvalues2 = _context.Customers.Where(x => x.CustomerCity == "Ankara")
                .Select(y => y.CustomerName + " " + y.CustomerSurname)
                .ToList();

            var intersectValues = cityvalues1.Intersect(cityvalues2).ToList();
            return View(intersectValues);
        }
        public IActionResult CustomerCastExample()
        {
            var values = _context.Customers.ToList();
            ViewBag.v = values;
            return View();
        }
        public IActionResult CustomerOffTypeExample()
        {

            var values = _context.Customers.ToList();
            ViewBag.v1 = values;
            return View();
        }
        public IActionResult CustomerWithByQuaryble()
        {
            using (var context = new StoreContext())
            {
                IQueryable<Customer> query = context.Customers;

                if (true)
                {
                    query = query.Where(c => c.CustomerCity == "İstanbul");
                }

                var customerList = query.ToList();


                return View(customerList);
            }
        }

        public IActionResult CustomerLİstIndex()
        {
            var customers = _context.Customers
                .ToList()
                .Select((c, index) => new
                {
                    SiraNo = index + 1,
                    c.CustomerName,
                    c.CustomerSurname,
                    c.CustomerCity

                })
                .ToList();
            return View(customers);
        }



    }
}
