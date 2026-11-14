
using Formula1Web.Models;
using Formula1Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System;
using System.Diagnostics;

namespace Formula1Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;

        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Надсилання Email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(
            EmailMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            await _emailSender.SendEmailAsync(
                model.Email,
                "Message from Formula 1 Website",
                model.Message);

            ViewBag.Message = "Email sent successfully!";

            return View("Index");
        }

        // Зміна мови сайту
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(
            string culture,
            string returnUrl = "/")
        {
            // Дозволяємо тільки українську та англійську
            if (culture != "uk" && culture != "en")
            {
                culture = "uk";
            }

            // Зберігаємо вибрану мову в cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                });

            // Повертаємо користувача на попередню сторінку
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
