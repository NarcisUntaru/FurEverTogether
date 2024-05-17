using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.Models;
using FurEver_Together.DataModels;
using AutoMapper;
using FurEver_Together.Interfaces;

namespace FurEver_Together.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVolunteerRepository _VolunteerRepository;

        public VolunteerController(IMapper mapper, IVolunteerRepository VolunteerRepository)
        {
            _mapper = mapper;
            _VolunteerRepository = VolunteerRepository;
        }

        public IActionResult Index()
        {
            var Volunteer = _VolunteerRepository.GetAll();
            var VolunteerModels = _mapper.Map<List<VolunteerViewModel>>(Volunteer);
            return View(VolunteerModels);
        }

        [HttpPost]
        public IActionResult Add(VolunteerViewModel VolunteerViewModel)
        {
            if (ModelState.IsValid)
            {
                _VolunteerRepository.Add(_mapper.Map<Volunteer>(VolunteerViewModel));
                _VolunteerRepository.Save();
            }
            return View();
        }

        public IActionResult Edit(int VolunteerId)
        {
            var Volunteer = _VolunteerRepository.GetById(VolunteerId);
            return View(_mapper.Map<VolunteerViewModel>(Volunteer));
        }

        [HttpPost]
        public IActionResult Edit(VolunteerViewModel? VolunteerViewModel)
        {
            if (ModelState.IsValid)
            {
                _VolunteerRepository.Update(_mapper.Map<Volunteer>(VolunteerViewModel));
                _VolunteerRepository.Save();
                return RedirectToAction("Index");
            }
            return View(VolunteerViewModel);
        }

        public IActionResult Delete(int VolunteerId)
        {
            var Volunteer = _VolunteerRepository.GetById(VolunteerId);
            _VolunteerRepository.Delete(Volunteer);
            _VolunteerRepository.Save();
            return RedirectToAction("Index");
        }
    }
}