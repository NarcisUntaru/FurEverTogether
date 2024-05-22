using Microsoft.AspNetCore.Mvc;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FurEver_Together.Services.Interfaces;
using System.Security.Claims;

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
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(VolunteerViewModel volunteerViewModel)
        {
            if (ModelState.IsValid)
            {
                var volunteer = _mapper.Map<Volunteer>(volunteerViewModel);
                volunteer.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _volunteerService.Add(volunteer);
                return RedirectToAction("Index");
            }
            return View(volunteerViewModel);
        }
    }
}
