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
    public class CatsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICatRepository _catRepository;

        public CatsController(IMapper mapper, ICatRepository catRepository)
        {
            _mapper = mapper;
            _catRepository = catRepository;
        }

        public IActionResult Index()
        {
            var Cats = _catRepository.GetAll();
            var CatModels = _mapper.Map<List<CatViewModel>>(Cats);
            return View(CatModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CatViewModel CatViewModel)
        {
            if (ModelState.IsValid)
            {
                _catRepository.Add(_mapper.Map<Cat>(CatViewModel));
                _catRepository.Save();
                return RedirectToAction("Index");
            }
            return View(CatViewModel);
        }

        public IActionResult Edit(int CatId)
        {
            var Cat = _catRepository.GetById(CatId);
            return View(_mapper.Map<CatViewModel>(Cat));
        }

        [HttpPost]
        public IActionResult Edit(CatViewModel? CatViewModel)
        {
            if (ModelState.IsValid)
            {
                _catRepository.Update(_mapper.Map<Cat>(CatViewModel));
                _catRepository.Save();
                return RedirectToAction("Index");
            }
            return View(CatViewModel);
        }

        public IActionResult Delete(int CatId)
        {
            var Cat = _catRepository.GetById(CatId);
            _catRepository.Delete(Cat);
            _catRepository.Save();
            return RedirectToAction("Index");
        }
    }
}