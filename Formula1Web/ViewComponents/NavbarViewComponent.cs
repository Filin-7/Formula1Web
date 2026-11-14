
using Microsoft.AspNetCore.Mvc;

namespace Formula1Web.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
