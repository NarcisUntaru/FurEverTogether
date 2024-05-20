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
using System.Diagnostics;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVolunteerRepository _VolunteerRepository;
        private readonly IUserRepository _UserRepository;

        public VolunteerController(IMapper mapper, IVolunteerRepository VolunteerRepository, IUserRepository UserRepository)
        {
            _mapper = mapper;
            _VolunteerRepository = VolunteerRepository;
            _UserRepository = UserRepository;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(VolunteerViewModel VolunteerViewModel)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var volunteer = _mapper.Map<Volunteer>(VolunteerViewModel);
                volunteer.UserId = currentUserId;
                _VolunteerRepository.Add(volunteer);
                _VolunteerRepository.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}