using Microsoft.AspNetCore.Mvc;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FurEver_Together.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;  // Add this for async support

namespace FurEver_Together.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IContactUsService _contactUsService;
        private readonly EmailService _emailService;  // inject EmailService

        public ContactUsController(IMapper mapper, IContactUsService contactUsService, EmailService emailService)
        {
            _mapper = mapper;
            _contactUsService = contactUsService;
            _emailService = emailService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ContactUsViewModel contactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                var contactUs = _mapper.Map<ContactUs>(contactUsViewModel);
                contactUs.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _contactUsService.Add(contactUs);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                string subject = $"New Contact Us Message";
                string body = $@"
            <p>You have a new contact us submission:</p>
            <p><strong>User ID:</strong> {userId}</p>
            <p><strong>Message:</strong> {contactUsViewModel.Message}</p>
        ";
                TempData["SuccessMessage"] = "Message successfully sent!";
                await _emailService.SendEmailAsync("narcis.alexandru02@gmail.com", subject, body);

                return RedirectToAction("Index");
            }
            return View(contactUsViewModel);
        }
    }
}
