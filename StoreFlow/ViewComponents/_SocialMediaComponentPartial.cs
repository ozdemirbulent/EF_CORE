using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents
{
    public class _SocialMediaComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
