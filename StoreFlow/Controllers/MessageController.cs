using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;

namespace StoreFlow.Controllers
{
    public class MessageController : Controller
    {
        private readonly StoreContext _context;

        public MessageController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult MessageList()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        public IActionResult AsNoTracking()
        {
            var values = _context.Messages.Where(x => x.SenderNameSurname.StartsWith("A")).AsNoTracking().ToList();
            return View(values);

            /*
             * hem a harfi ile başlayan gönderen isimli mesajları getir
             * hem de AsNoTracking kullanımına değinildi.
             */
        }
        public IActionResult MessageDetail(int id)
        {
            var message = _context.Messages.FirstOrDefault(x => x.MessageId == id);

            if (message == null)
            {
                return NotFound();
            }

       
            message.IsRead = true;
            _context.SaveChanges();

            return View(message);
        }



    }
}
