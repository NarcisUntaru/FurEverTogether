using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IContactUsRepository _ContactUsRepository;
        private readonly IUserRepository _UserRepository;

        public ContactUsController(IMapper mapper, IContactUsRepository ContactUsRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _ContactUsRepository = ContactUsRepository;
            _UserRepository = userRepository;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ContactUsViewModel ContactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var contactUs = _mapper.Map<ContactUs>(ContactUsViewModel);

                contactUs.UserId = currentUserId;
                _ContactUsRepository.Add(contactUs);
                _ContactUsRepository.Save();
                return RedirectToAction("Index");
            }

            return View(ContactUsViewModel);
        }
    }
}