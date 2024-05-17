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
    public class ContactUsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IContactUsRepository _ContactUsRepository;

        public ContactUsController(IMapper mapper, IContactUsRepository ContactUsRepository)
        {
            _mapper = mapper;
            _ContactUsRepository = ContactUsRepository;
        }

        public IActionResult Index()
        {
            var ContactUss = _ContactUsRepository.GetAll();
            var ContactUsModels = _mapper.Map<List<ContactUsViewModel>>(ContactUss);
            return View(ContactUsModels);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ContactUsViewModel ContactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                _ContactUsRepository.Add(_mapper.Map<ContactUs>(ContactUsViewModel));
                _ContactUsRepository.Save();
                return RedirectToAction("Index");
            }
            return View(ContactUsViewModel);
        }

        public IActionResult Edit(int ContactUsId)
        {
            var ContactUs = _ContactUsRepository.GetById(ContactUsId);
            return View(_mapper.Map<ContactUsViewModel>(ContactUs));
        }

        [HttpPost]
        public IActionResult Edit(ContactUsViewModel? ContactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                _ContactUsRepository.Update(_mapper.Map<ContactUs>(ContactUsViewModel));
                _ContactUsRepository.Save();
                return RedirectToAction("Index");
            }
            return View(ContactUsViewModel);
        }

        public IActionResult Delete(int ContactUsId)
        {
            var ContactUs = _ContactUsRepository.GetById(ContactUsId);
            _ContactUsRepository.Delete(ContactUs);
            _ContactUsRepository.Save();
            return RedirectToAction("Index");
        }
    }
}