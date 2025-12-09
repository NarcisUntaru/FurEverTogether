using FurEver_Together.DataModels;
using FurEver_Together.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurEver_Together.Controllers
{
    public class SheltersController : Controller
    {
        private readonly IShelterService _shelterService;

        public SheltersController(IShelterService shelterService)
        {
            _shelterService = shelterService;
        }

        // GET: Shelters
        public async Task<IActionResult> Index()
        {
            var shelters = await _shelterService.GetAllSheltersAsync();
            return View(shelters);
        }

        // GET: Shelters/Map
        public async Task<IActionResult> Map()
        {
            var shelters = await _shelterService.GetAllSheltersAsync();
            return View(shelters);
        }

        // GET: Shelters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var shelter = await _shelterService.GetShelterWithPetsAsync(id.Value);
            if (shelter == null)
                return NotFound();

            return View(shelter);
        }

        // GET: Shelters/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shelters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Shelter shelter)
        {
            if (!ModelState.IsValid)
                return View(shelter);

            await _shelterService.AddShelterAsync(shelter);
            return RedirectToAction(nameof(Index));
        }

        // GET: Shelters/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var shelter = await _shelterService.GetShelterByIdAsync(id.Value);
            if (shelter == null)
                return NotFound();

            return View(shelter);
        }

        // POST: Shelters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, Shelter shelter)
        {
            if (id != shelter.ShelterId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(shelter);

            await _shelterService.UpdateShelterAsync(shelter);
            return RedirectToAction(nameof(Index));
        }

        // GET: Shelters/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var shelter = await _shelterService.GetShelterByIdAsync(id.Value);
            if (shelter == null)
                return NotFound();

            return View(shelter);
        }

        // POST: Shelters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shelterService.DeleteShelterAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // API endpoint for map data
        [HttpGet]
        public async Task<IActionResult> GetSheltersJson()
        {
            var shelters = await _shelterService.GetAllSheltersAsync();
            return Json(shelters);
        }
    }
}