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
        private readonly IPdfExportService _pdfExportService;

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
            _pdfExportService = pdfExportService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adoptionRequests = await _adoptionService.GetAdoptionsByUserIdAsync(userId);
            return View(adoptionRequests);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            var matchPercentage = await CalculateMatchPercentageAsync(adoption);
            ViewData["MatchPercentage"] = matchPercentage;

            return View(adoption);
        }

        public async Task<IActionResult> Create()
        {
            var pets = await _petService.GetAllPetsAsync();
            var viewModel = CreateAdoptionViewModel(pets);
            return View(viewModel);
        }

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            var pets = await _petService.GetAllPetsAsync();
            var viewModel = CreateEditViewModel(adoption, pets);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null) return NotFound();

            return View(adoption);
        }

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

            await UpdateAdoptionForUser(adoption, user.Id);
            return RedirectToAction("Index", "Adoptions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
            if (adoption == null) return NotFound();

            await ProcessApproval(adoption);
            await SendApprovalEmail(adoption);

            return Redirect("/Identity/Account/Manage#adoption-requests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
            if (adoption == null) return NotFound();

            await ProcessRejection(adoption);
            await SendRejectionEmail(adoption);

            return Redirect("/Identity/Account/Manage#adoption-requests");
        }

        [HttpGet]
        public async Task<IActionResult> ExportToPdf()
        {
            var adoptionRequests = await _adoptionService.GetAllAdoptionsAsync();
            var pdfBytes = _pdfExportService.GenerateAdoptionRequestsPdf(adoptionRequests);

            var fileName = $"AdoptionRequests_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // Private helper methods

        private async Task<double> CalculateMatchPercentageAsync(Adoption adoption)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || adoption.Pet == null)
                return 0;

            return await _matchingService.GetMatchPercentageForPetAndUserAsync(adoption.Pet.PetId, user.Id);
        }

        private static AdoptionViewModel CreateAdoptionViewModel(IEnumerable<Pet> pets)
        {
            var availablePets = pets
                .Where(p => !p.IsAdopted && p.Adoption == null)
                .Select(p => new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.Name
                })
                .ToList();

            return new AdoptionViewModel { Pets = availablePets };
        }

        private static AdoptionViewModel CreateEditViewModel(Adoption adoption, IEnumerable<Pet> pets)
        {
            return new AdoptionViewModel
            {
                Id = adoption.Id,
                AdoptionDate = adoption.AdoptionDate,
                PetId = adoption.PetId,
                Pets = pets.Select(p => new SelectListItem
                {
                    Value = p.PetId.ToString(),
                    Text = p.Name
                }).ToList()
            };
        }

        private async Task UpdateAdoptionForUser(Adoption adoption, string userId)
        {
            adoption.UserId = userId;
            adoption.Status = ApplicationStatus.Pending;
            adoption.RequestDate = DateTime.Now;
            await _adoptionService.UpdateAdoptionAsync(adoption);
        }

        private async Task ProcessApproval(Adoption adoption)
        {
            adoption.Status = ApplicationStatus.Approved;
            adoption.AdoptionDate = DateTime.Now;

            await MarkPetAsAdopted(adoption);
            await _adoptionService.UpdateAdoptionAsync(adoption);
        }

        private async Task MarkPetAsAdopted(Adoption adoption)
        {
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
        }

        private async Task ProcessRejection(Adoption adoption)
        {
            adoption.Status = ApplicationStatus.Rejected;
            adoption.AdoptionDate = DateTime.Now;
            await _adoptionService.UpdateAdoptionAsync(adoption);
        }

        private async Task SendApprovalEmail(Adoption adoption)
        {
            const string message = "Great news! Your adoption application has been approved, and we are excited to welcome you as the new family for your chosen pet. Our team will contact you shortly with the next steps to finalize the adoption and prepare your new furry friend for their new home. We can't wait to see the joy this adoption will bring to your life. Thank you for giving a shelter pet a second chance!";
            const string subject = "Adoption Application Approved";

            var body = BuildEmailBody(message, adoption, "Approved");
            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);
        }

        private async Task SendRejectionEmail(Adoption adoption)
        {
            const string message = "We regret to inform you that, after careful consideration, your adoption application has not been approved at this time. We understand this may be disappointing news, but please know that our decision was made with the pet's best interest in mind. If you would like, feel free to contact us for more detailed feedback. Thank you for your interest and compassion toward animals in need.";
            const string subject = "Adoption Application Rejected";

            var body = BuildEmailBody(message, adoption, "Rejected");
            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);
        }

        private static string BuildEmailBody(string message, Adoption adoption, string status)
        {
            return $@"
                <p>{message}</p>
                <p><strong>Pet Name:</strong> {adoption.Pet.Name}</p>
                <p><strong>Adoption ID:</strong> {adoption.Id}</p>
                <p><strong>Status:</strong> {status}</p>
                <p><strong>Date:</strong> {DateTime.Now}</p>
            ";
        }
    }
}