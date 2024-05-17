using Microsoft.AspNetCore.Mvc;

namespace FurEver_Together.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
