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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_volunteerService.HasSubmitted(userId))
            {
                return View("AlreadyApplied");
            }
            var viewModel = new VolunteerViewModel
            {
                HoursPerWeek = 1
            };
            return View(viewModel);
        }
        [Authorize]
        public IActionResult Details(int id)
        {
            var volunteer = _volunteerService.GetById(id);

            if (volunteer == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (volunteer.UserId != currentUserId)
            {
                return Forbid();
            }

            var viewModel = _mapper.Map<VolunteerViewModel>(volunteer);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(VolunteerViewModel volunteerViewModel)
        {
            if (ModelState.IsValid)
            {
                var volunteer = _mapper.Map<Volunteer>(volunteerViewModel);
                volunteer.Status = ApplicationStatus.Pending;
                volunteer.RequestDate = DateTime.Now;
                volunteer.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _volunteerService.Add(volunteer);
                return RedirectToAction("Index");
            }

            return View("Index", volunteerViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var volunteer = _volunteerService.GetById(id);

            if (volunteer == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (volunteer.UserId != currentUserId)
            {
                return Forbid();
            }

            _volunteerService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Approve(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            volunteer.Status = ApplicationStatus.Approved;
            volunteer.RespondDate = DateTime.Now;

            await _volunteerService.UpdateAsync(volunteer);

            string message = "Great news! Your volunteer application has been approved, and we are excited to welcome you to our team. Our coordinators will contact you shortly with the next steps and more details about your involvement. Thank you for your willingness to help and make a difference!";
            string subject = "Volunteer Application Approved";
            string body = $@"
        <p>{message}</p>
        <p><strong>Volunteer Name:</strong> {volunteer.FullName}</p>
        <p><strong>Application ID:</strong> {volunteer.Id}</p>
        <p><strong>Status:</strong> Approved</p>
        <p><strong>Date:</strong> {DateTime.Now}</p>
    ";

            // Hardcoded email
            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);

            return Redirect("/Identity/Account/Manage#admin-panel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Reject(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            volunteer.Status = ApplicationStatus.Rejected;
            volunteer.RespondDate = DateTime.Now;

            await _volunteerService.UpdateAsync(volunteer);

            string message = "We regret to inform you that, after careful consideration, your volunteer application has not been approved at this time. We appreciate your interest and hope you will consider applying again in the future.";
            string subject = "Volunteer Application Rejected";
            string body = $@"
        <p>{message}</p>
        <p><strong>Volunteer Name:</strong> {volunteer.FullName}</p>
        <p><strong>Application ID:</strong> {volunteer.Id}</p>
        <p><strong>Status:</strong> Rejected</p>
        <p><strong>Date:</strong> {DateTime.Now}</p>
    ";

            // Hardcoded email
            await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);

            return Redirect("/Identity/Account/Manage#admin-panel");
        }

    }
}
