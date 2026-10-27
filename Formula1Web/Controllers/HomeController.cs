using Formula1Web.Models;
using Formula1Web.Services;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailMessageViewModel model)
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