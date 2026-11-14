
using Microsoft.AspNetCore.Mvc;

namespace Formula1Web.Controllers
{
    public class FormulaController : Controller
    {
        // Сторінка перегонів
        public IActionResult Racing()
        {
            return View();
        }

        // Сторінка команд
        public IActionResult Teams()
        {
            return View();
        }

        // Сторінка чемпіонів
        public IActionResult Champions()
        {
            return View();
        }
    }
}
