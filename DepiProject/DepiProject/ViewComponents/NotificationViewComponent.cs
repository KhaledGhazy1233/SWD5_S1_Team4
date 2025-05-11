using Microsoft.AspNetCore.Mvc;

namespace DepiProject.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
