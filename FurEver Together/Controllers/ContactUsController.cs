using Microsoft.AspNetCore.Mvc;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FurEver_Together.Services.Interfaces;
using System.Security.Claims;

namespace FurEver_Together.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IMapper mapper, IContactUsService contactUsService)
        {
            _mapper = mapper;
            _contactUsService = contactUsService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(ContactUsViewModel contactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                var contactUs = _mapper.Map<ContactUs>(contactUsViewModel);
                contactUs.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _contactUsService.Add(contactUs);
                return RedirectToAction("Index");
            }
            return View(contactUsViewModel);
        }
    }
}
