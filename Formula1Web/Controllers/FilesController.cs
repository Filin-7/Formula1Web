
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Formula1Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        private readonly string[] _allowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".gif",
            ".pdf", ".txt"
        };

        public FilesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Сторінка зі списком файлів
        public IActionResult Index()
        {
            string folder = GetFilesFolder();

            var files = Directory.GetFiles(folder)
                .Select(Path.GetFileName)
                .Where(name => name != null)
                .Select(name => name!)
                .OrderBy(name => name)
                .ToList();

            return View(files);
        }

        // Форма завантаження файлу
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // Збереження файлу на сервері
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Select a file.");
                return View();
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("", "File is too large.");
                return View();
            }

            string extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "File type is not allowed.");
                return View();
            }

            string folder = GetFilesFolder();

            // Генеруємо безпечну унікальну назву
            string fileName = Guid.NewGuid().ToString("N")
                + extension;

            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction(nameof(Index));
        }

        // Перегляд вибраного файлу
        public IActionResult ViewFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName != Path.GetFileName(fileName))
            {
                return BadRequest();
            }

            string extension = Path.GetExtension(fileName)
                .ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
            {
                return BadRequest();
            }

            string folder = GetFilesFolder();
            string path = Path.Combine(folder, fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            string contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };

            return PhysicalFile(path, contentType);
        }

        private string GetFilesFolder()
        {
            string root = _environment.WebRootPath
                ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

            string folder = Path.Combine(root, "Files");

            Directory.CreateDirectory(folder);

            return folder;
        }
    }
}
