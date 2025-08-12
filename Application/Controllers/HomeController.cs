using Application.Common.Logging;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBooksService _booksService;
        public HomeController(ILogger<HomeController> logger, IBooksService booksService)
        {
            _logger = logger;
            _booksService = booksService;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationLog.Warn("Running Home Index");
            var abc = await _booksService.GetAllBooks();
            ApplicationLog.Warn("Retrived all books from db");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
