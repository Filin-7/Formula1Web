
using FluentValidation;
using Formula1Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Formula1Web.Controllers
{
    public class DriverController : Controller
    {
        private readonly IValidator<DriverRegistration> _validator;

        public DriverController(
            IValidator<DriverRegistration> validator)
        {
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new DriverRegistration());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            DriverRegistration model)
        {
            var result = await _validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ViewBag.Success =
                "Driver registration completed successfully!";

            ModelState.Clear();

            return View(new DriverRegistration());
        }
    }
}
