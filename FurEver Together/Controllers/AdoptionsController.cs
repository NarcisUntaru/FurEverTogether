using FurEver_Together.DataModels;
using FurEver_Together.Enums;
using FurEver_Together.Services.Interfaces;
using FurEver_Together.ViewModels;
using FurEverTogether.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FurEver_Together.Controllers
{
    public class AdoptionsController : Controller
    {
        private readonly IAdoptionService _adoptionService;
        private readonly IPetService _petService;
        private readonly IMatchingService _matchingService;
        private readonly UserManager<User> _userManager;
        private readonly IPdfExportService _pdfExportService;

        public AdoptionsController(
            IMatchingService matchingService,
            IAdoptionService adoptionService,
            IPetService petService,
            IPdfExportService pdfExportService,
            UserManager<User> userManager)
        {
            _matchingService = matchingService;
            _adoptionService = adoptionService;
            _petService = petService;
            _userManager = userManager;
            _pdfExportService = pdfExportService;
        }

        // GET: Adoptions
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adoptionRequests = await _adoptionService.GetAdoptionsByUserIdAsync(userId);

            return View(adoptionRequests);
        }

        // GET: Adoptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            double matchPercentage = 0;

            var user = await _userManager.GetUserAsync(User);
            if (user != null && adoption.Pet != null)
            {
                // Use MatchingService to calculate match percentage
                matchPercentage = await _matchingService.GetMatchPercentageForPetAndUserAsync(adoption.Pet.PetId, user.Id);
            }

            ViewData["MatchPercentage"] = matchPercentage;

            return View(adoption);
        }

        // GET: Adoptions/Create
        public async Task<IActionResult> Create()
        {
            var pets = await _petService.GetAllPetsAsync();

            var viewModel = new AdoptionViewModel
            {
                Pets = pets.Select(p => new SelectListItem { Value = p.PetId.ToString(), Text = p.Name }).ToList()
            };

            return View(viewModel);
        }

        // POST: Adoptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdoptionViewModel viewModel)
        {
                var adoption = new Adoption
                {
                    PetId = viewModel.PetId,
                    AdoptionDate = null, 
                    RequestDate = null, 
                };

                await _adoptionService.AddAdoptionAsync(adoption);
            return Redirect("/Identity/Account/Manage#admin-panel");
        }
        
        // GET: Adoptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            var pets = await _petService.GetAllPetsAsync();

            var viewModel = new AdoptionViewModel
            {
                Id = adoption.Id,
                AdoptionDate = adoption.AdoptionDate,
                PetId = adoption.PetId,
                Pets = pets.Select(p => new SelectListItem { Value = p.PetId.ToString(), Text = p.Name }).ToList()
            };

            return View(viewModel);
        }

        //// POST: Adoptions/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, AdoptionViewModel viewModel)
        //{
        //    if (id != viewModel.Id) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        var adoption = new Adoption
        //        {
        //            Id = viewModel.Id,
        //            AdoptionDate = viewModel.AdoptionDate,
        //            FreeTransportation = viewModel.FreeTransportation,
        //            AdoptionStory = viewModel.AdoptionStory,
        //            PetId = viewModel.PetId
        //        };

        //        try
        //        {
        //            await _adoptionService.UpdateAdoptionAsync(adoption);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            return NotFound();
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var pets = await _petService.GetAllPetsAsync();
        //    viewModel.Pets = pets.Select(p => new SelectListItem { Value = p.PetId.ToString(), Text = p.Name }).ToList();

        //    return View(viewModel);
        //}

        // GET: Adoptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            return View(adoption);
        }

        // POST: Adoptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adoptionService.DeleteAdoptionAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adopt(int petId)
        {
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var adoption = await _adoptionService.GetAdoptionByPetIdAsync(petId);
            if (adoption == null)
                return NotFound("Adoption record not found for the selected pet.");

            // Update the existing adoption record
            adoption.UserId = user.Id;
            adoption.Status = ApplicationStatus.Pending;
            adoption.RequestDate = DateTime.Now;

            await _adoptionService.UpdateAdoptionAsync(adoption);

            return RedirectToAction("Index", "Adoptions");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
            if (adoption == null) return NotFound();

            adoption.Status = ApplicationStatus.Approved;
            adoption.AdoptionDate = DateTime.Now;
            if (adoption.Pet != null)
            {
                adoption.Pet.IsAdopted = true;
            }
    {
        // If your adoption object doesn't include the Pet navigation property,
        // you might need to load it explicitly from the service/db context.
        var pet = await _petService.GetPetByIdAsync(adoption.PetId);
        if (pet != null)
        {
            pet.IsAdopted = true;
            await _petService.UpdatePetAsync(pet);
        }
    }
            await _adoptionService.UpdateAdoptionAsync(adoption);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
            if (adoption == null) return NotFound();

            adoption.Status = ApplicationStatus.Rejected;
            adoption.AdoptionDate = DateTime.Now;

            await _adoptionService.UpdateAdoptionAsync(adoption);

            return RedirectToAction("Index");
        }

        public IActionResult Export()
        {
            string html = "<h1>Hello from service</h1><p>This is a PDF.</p>";
            byte[] pdf = _pdfExportService.GeneratePdfFromHtml(html, "My PDF");

            return File(pdf, "application/pdf", "output.pdf");
        }

    }
}
