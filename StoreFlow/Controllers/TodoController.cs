using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;
using StoreFlow.Entities;

namespace StoreFlow.Controllers
{
    public class TodoController : Controller
    {
        private readonly StoreContext _context;

        public TodoController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateToDo()
        {
            var todos = new List<Todo>
            {
                new Todo { Description = "Mail Gönder ", Status = false , Priority = "Birincil"},
                new Todo { Description = "Rapor Hazırla", Status = true , Priority = "İkincil"},
                new Todo { Description = "Toplantıya Katıl", Status = false , Priority = "Üçüncül"},

            };
            _context.todo.AddRange(todos);
            _context.SaveChanges();
            return View();
        }

        public IActionResult ToDoAggreate()
        {
            var priorityFirstlyTodo = _context.todo.Where(t => t.Priority == "Üçüncül").Select(t => t.Description).ToList();

            var result = priorityFirstlyTodo.Aggregate(string.Empty, (acc, desc) => acc + $"<li >{desc}</li>");

            ViewBag.result = result;

            return View();
        }

        public IActionResult InCompleteTask()
        {
            var values = _context.todo.Where(x => x.Status == false)
                .Select(y => y.Description)
                .ToList().Append("Gün Sonunda Tüm Görevleri Kontrol Etmeyi Unutmayın !")
                .ToList();
            return View(values);
        }
        public IActionResult PrependMethod()
        {
            var values = _context.todo.Where(x => x.Status == false)
                .Select(y => y.Description)
                .ToList().Prepend("Hazırsanız Başlıyoruz !")
                .ToList();
            return View(values);
        }

        public IActionResult ToDoChunk()
        {
            var values = _context.todo.Where(x => x.Status == false).ToList().Chunk(2).ToList();
            return View(values);
        }
        public IActionResult ToDoConcat()
        {
            var values = _context.todo
                .Where(x => x.Priority == "Birincil")
                .ToList()
                .Concat(

                _context.todo
                    .Where(y => y.Priority == "Üçüncül")
                    .ToList()
                ).ToList();
            return View(values);
        }

        public IActionResult ToDoUnion()
        {
            var values = _context.todo.Where(x => x.Priority == "Birincil").ToList();
            var values2 = _context.todo.Where(y => y.Priority == "İkincil").ToList();
            var result = values.UnionBy(values2, x => x.Description).ToList();
            return View(result);
        }
        public IActionResult ToDoDetail(int id)
        {
            var todo = _context.todo.FirstOrDefault(x => x.TodoId == id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }


    }
}