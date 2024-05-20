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
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Controllers
{
    public class AdoptionsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAdoptionRepository _adoptionRepository;

        public AdoptionsController(IMapper mapper, IAdoptionRepository adoptionRepository)
        {
            _mapper = mapper;
            _adoptionRepository = adoptionRepository;
        }

        public IActionResult Index()
        {
            var Adoptiones = _adoptionRepository.GetAll();
            var AdoptionModels = _mapper.Map<List<AdoptionViewModel>>(Adoptiones);
            return View(AdoptionModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string age)
        {
            if (ModelState.IsValid)
            {
                var adoption = new Adoption
                {
                    AnimalType = "",
                    Breed = "",
                    Age = age,
                    Gender = "",
                    Description = "",
                    ImageURL = "",
                    //UserId = "1",
                    //CatId = "1",
                    //DogId = "1"
                };
                _adoptionRepository.Add(adoption);
                _adoptionRepository.Save();
                return RedirectToAction("Index");
            }
            return View(age);
        }

        public IActionResult Edit(int AdoptionId)
        {
            var Adoption = _adoptionRepository.GetById(AdoptionId);
            return View(_mapper.Map<AdoptionViewModel>(Adoption));
        }

        [HttpPost]
        public IActionResult Edit(AdoptionViewModel? AdoptionViewModel)
        {
            if (ModelState.IsValid)
            {
                _adoptionRepository.Update(_mapper.Map<Adoption>(AdoptionViewModel));
                _adoptionRepository.Save();
                return RedirectToAction("Index");
            }
            return View(AdoptionViewModel);
        }

        public IActionResult Delete(int AdoptionId)
        {
            var Adoption = _adoptionRepository.GetById(AdoptionId);
            _adoptionRepository.Delete(Adoption);
            _adoptionRepository.Save();
            return RedirectToAction("Index");
        }
    }
}