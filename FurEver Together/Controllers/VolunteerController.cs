using Microsoft.AspNetCore.Mvc;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FurEver_Together.Services.Interfaces;
using System.Security.Claims;
using FurEver_Together.Enums;
using FurEver_Together.Services;

namespace FurEver_Together.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVolunteerService _volunteerService;
        private readonly EmailService _emailService;

        public VolunteerController(IMapper mapper, IVolunteerService volunteerService, EmailService emailService)
        {
            _mapper = mapper;
            _volunteerService = volunteerService;
            _emailService = emailService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            return _volunteerService.HasSubmitted(userId)
                ? View("AlreadyApplied")
                : View(CreateDefaultViewModel());
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var volunteer = _volunteerService.GetById(id);
            var authResult = ValidateVolunteerAccess(volunteer);

            return authResult ?? View(_mapper.Map<VolunteerViewModel>(volunteer));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(VolunteerViewModel volunteerViewModel)
        {
            return ModelState.IsValid
                ? ProcessVolunteerAddition(volunteerViewModel)
                : View("Index", volunteerViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var volunteer = _volunteerService.GetById(id);
            var authResult = ValidateVolunteerAccess(volunteer);

            return authResult ?? ExecuteDelete(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Approve(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            return volunteer == null
                ? NotFound()
                : await ProcessStatusChange(volunteer, ApplicationStatus.Approved, GetApprovalEmailContent());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Reject(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            return volunteer == null
                ? NotFound()
                : await ProcessStatusChange(volunteer, ApplicationStatus.Rejected, GetRejectionEmailContent());
        }

        // Private helper methods
        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private VolunteerViewModel CreateDefaultViewModel() => new VolunteerViewModel { HoursPerWeek = 1 };

        private IActionResult ValidateVolunteerAccess(Volunteer volunteer)
        {
            return volunteer == null ? NotFound()
                : volunteer.UserId != GetCurrentUserId() ? Forbid()
                : null;
        }

        private IActionResult ProcessVolunteerAddition(VolunteerViewModel volunteerViewModel)
        {
            var volunteer = _mapper.Map<Volunteer>(volunteerViewModel);
            volunteer.Status = ApplicationStatus.Pending;
            volunteer.RequestDate = DateTime.Now;
            volunteer.UserId = GetCurrentUserId();
            _volunteerService.Add(volunteer);
            return RedirectToAction("Index");
        }

        private IActionResult ExecuteDelete(int id)
        {
            _volunteerService.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IActionResult> ProcessStatusChange(Volunteer volunteer, ApplicationStatus status, EmailContent emailContent)
        {
            volunteer.Status = status;
            volunteer.RespondDate = DateTime.Now;
            await _volunteerService.UpdateAsync(volunteer);
            await SendStatusEmail(volunteer, emailContent);
            return Redirect("/Identity/Account/Manage#admin-panel");
        }

        private async Task SendStatusEmail(Volunteer volunteer, EmailContent content)
        {
            string body = BuildEmailBody(content.Message, volunteer);
            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", content.Subject, body);
        }

        private string BuildEmailBody(string message, Volunteer volunteer)
        {
            return $@"
                <p>{message}</p>
                <p><strong>Volunteer Name:</strong> {volunteer.FullName}</p>
                <p><strong>Application ID:</strong> {volunteer.Id}</p>
                <p><strong>Status:</strong> {volunteer.Status}</p>
                <p><strong>Date:</strong> {DateTime.Now}</p>
            ";
        }

        private EmailContent GetApprovalEmailContent() => new EmailContent
        {
            Subject = "Volunteer Application Approved",
            Message = "Great news! Your volunteer application has been approved, and we are excited to welcome you to our team. Our coordinators will contact you shortly with the next steps and more details about your involvement. Thank you for your willingness to help and make a difference!"
        };

        private EmailContent GetRejectionEmailContent() => new EmailContent
        {
            Subject = "Volunteer Application Rejected",
            Message = "We regret to inform you that, after careful consideration, your volunteer application has not been approved at this time. We appreciate your interest and hope you will consider applying again in the future."
        };

        private class EmailContent
        {
            public string Subject { get; set; }
            public string Message { get; set; }
        }
    }
}