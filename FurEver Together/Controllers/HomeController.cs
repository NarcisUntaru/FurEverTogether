using FurEver_Together.Enums;
using FurEver_Together.Services;
using FurEver_Together.Services.Interfaces;
using FurEver_Together.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FurEver_Together.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdoptionService _adoptionService;

        public HomeController(ILogger<HomeController> logger, IAdoptionService adoptionService)
        {
            _logger = logger;
            _adoptionService = adoptionService;
        }

        public async Task<IActionResult> Index()
        {
            var adoptions2024 = await _adoptionService.GetApprovedAdoptions2024Async();
            var adoptionsAllTime = await _adoptionService.GetApprovedAdoptionsAllTimeAsync();

            ViewData["Adoptions2024"] = adoptions2024;
            ViewData["AdoptionsAllTime"] = adoptionsAllTime;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}