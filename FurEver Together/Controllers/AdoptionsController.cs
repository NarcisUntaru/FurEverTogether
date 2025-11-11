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
        private readonly EmailService _emailService;


        public AdoptionsController(
            IMatchingService matchingService,
            IAdoptionService adoptionService,
            IPetService petService,
            IPdfExportService pdfExportService,
            UserManager<User> userManager,
            EmailService emailService)
        {
            _matchingService = matchingService;
            _adoptionService = adoptionService;
            _petService = petService;
            _userManager = userManager;
            _emailService = emailService;
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

            var availablePets = pets
                .Where(p => !p.IsAdopted && p.Adoption == null) 
                .Select(p => new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.Name
                })
                .ToList();

            var viewModel = new AdoptionViewModel
            {
                Pets = availablePets
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

            var pet = await _petService.GetPetByIdAsync(adoption.PetId);
            if (pet != null)
            {
                pet.IsAdopted = true;
                await _petService.UpdatePetAsync(pet);
            }

            await _adoptionService.UpdateAdoptionAsync(adoption);

            string message = "Great news! Your adoption application has been approved, and we are excited to welcome you as the new family for your chosen pet. Our team will contact you shortly with the next steps to finalize the adoption and prepare your new furry friend for their new home. We can't wait to see the joy this adoption will bring to your life. Thank you for giving a shelter pet a second chance!";

            string subject = "Adoption Application Approved";
            string body = $@"
        <p>{message}</p>
        <p><strong>Pet Name:</strong> {adoption.Pet.Name}</p>
        <p><strong>Adoption ID:</strong> {adoption.Id}</p>
        <p><strong>Status:</strong> Approved</p>
        <p><strong>Date:</strong> {DateTime.Now}</p>
    ";

            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);

            return Redirect("/Identity/Account/Manage#adoption-requests");
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

            string message = "We regret to inform you that, after careful consideration, your adoption application has not been approved at this time. We understand this may be disappointing news, but please know that our decision was made with the pet’s best interest in mind. If you would like, feel free to contact us for more detailed feedback. Thank you for your interest and compassion toward animals in need.";

            string subject = "Adoption Application Rejected";
            string body = $@"
        <p>{message}</p>
        <p><strong>Pet Name:</strong> {adoption.Pet.Name}</p>
        <p><strong>Adoption ID:</strong> {adoption.Id}</p>
        <p><strong>Status:</strong> Rejected</p>
        <p><strong>Date:</strong> {DateTime.Now}</p>
    ";

            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);

            return Redirect("/Identity/Account/Manage#adoption-requests");
        }

    }
}
