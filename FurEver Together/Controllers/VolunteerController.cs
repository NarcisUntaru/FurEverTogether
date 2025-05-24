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

        public VolunteerController(IMapper mapper, IVolunteerService volunteerService)
        {
            _mapper = mapper;
            _volunteerService = volunteerService;
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
        [Authorize(Roles = "Administrator")] // or appropriate roles
        public IActionResult Approve(int id)
        {
            var volunteer = _volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            volunteer.Status = ApplicationStatus.Approved;
            volunteer.RespondDate = DateTime.Now;
            _volunteerService.Update(volunteer);
            return RedirectToAction("Index", "Volunteer");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Reject(int id)
        {
            var volunteer = _volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            volunteer.Status = ApplicationStatus.Rejected;
            volunteer.RespondDate = DateTime.Now;
            _volunteerService.Update(volunteer); //
            return RedirectToAction("Index", "Volunteer");
        }
    }
}
