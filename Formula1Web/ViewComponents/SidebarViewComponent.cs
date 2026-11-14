
using Microsoft.AspNetCore.Mvc;

namespace Formula1Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string layout = "Main")
        {
            if (layout == "Compact")
            {
                return View("Compact");
            }

            return View("Default");
        }
    }
}
