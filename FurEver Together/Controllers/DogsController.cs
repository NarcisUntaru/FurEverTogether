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
    public class DogsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDogRepository _DogRepository;

        public DogsController(IMapper mapper, IDogRepository DogRepository)
        {
            _mapper = mapper;
            _DogRepository = DogRepository;
        }

        public IActionResult Index()
        {
            var Dogs = _DogRepository.GetAll();
            var DogModels = _mapper.Map<List<DogViewModel>>(Dogs);
            return View(DogModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(DogViewModel DogViewModel)
        {
            if (ModelState.IsValid)
            {
                _DogRepository.Add(_mapper.Map<Dog>(DogViewModel));
                _DogRepository.Save();
                return RedirectToAction("Index");
            }
            return View(DogViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int DogId)
        {
            var Dog = _DogRepository.GetById(DogId);
            return View(_mapper.Map<DogViewModel>(Dog));
        }

        [HttpPost]
        public IActionResult Edit(DogViewModel DogViewModel)
        {
            if (ModelState.IsValid)
            {
                _DogRepository.Update(_mapper.Map<Dog>(DogViewModel));
                _DogRepository.Save();
                return RedirectToAction("Index");
            }
            return View(DogViewModel);
        }

        public IActionResult Delete(int DogId)
        {
            var Dog = _DogRepository.GetById(DogId);
            _DogRepository.Delete(Dog);
            _DogRepository.Save();
            return RedirectToAction("Index");
        }
    }
}