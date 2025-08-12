using Application.Common;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class LogController : Controller
    {
        private readonly IApplicationLogsService _applicationLogsService;

        public LogController(IApplicationLogsService applicationLogsService)
        {
            _applicationLogsService = applicationLogsService;
        }

        public IActionResult Index()
        {
            var model = _applicationLogsService.GetLogs().Result;
            return View(model.OrderByDescending(time => time.TimeStamp));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0) 
            {
                TempData["ErrorMessage"] = "Log ID was not provided.";
                return View(AppConstants.NotFoundView);
            }
            var logDetails = _applicationLogsService.GetLogsById(id).Result;
            if(logDetails == null)
            {
                TempData["ErrorMessage"] = $"No logs found with ID {id}.";
                return View(AppConstants.NotFoundView);
            }

            ViewBag.Message = logDetails.Message;
            ViewBag.Exception = logDetails.Exception;
            ViewBag.LogEvent = logDetails.LogEvent;

            return View();
        }
    }
}

