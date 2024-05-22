using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Services.Interfaces;
using FurEver_Together.ViewModels;

namespace FurEver_Together.Controllers
{
    public class AdoptionsController : Controller
    {
        private readonly IAdoptionService _adoptionService;
        private readonly IDogService _dogService;
        private readonly ICatService _catService;

        public AdoptionsController(IAdoptionService adoptionService, IDogService dogService, ICatService catService)
        {
            _adoptionService = adoptionService;
            _dogService = dogService;
            _catService = catService;
        }

        // GET: Adoptions
        public async Task<IActionResult> Index()
        {
            var adoptions = await _adoptionService.GetAllAdoptionsAsync();
            return View(adoptions);
        }

        // GET: Adoptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null)
            {
                return NotFound();
            }

            return View(adoption);
        }

        // GET: Adoptions/Create
        public async Task<IActionResult> Create()
        {
            var dogs = await _dogService.GetAllDogsAsync();
            var cats = await _catService.GetAllCatsAsync();

            var viewModel = new AdoptionViewModel
            {
                Dogs = dogs.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList(),
                Cats = cats.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };

            return View(viewModel);
        }

        // POST: Adoptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdoptionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var adoption = new Adoption
                {
                    Id = viewModel.Id,
                    AdoptionDate = viewModel.AdoptionDate,
                    FreeTransportation = viewModel.FreeTransportation,
                    AdoptionStory = viewModel.AdoptionStory,
                    DogId = viewModel.DogId,
                    CatId = viewModel.CatId
                };

                await _adoptionService.AddAdoptionAsync(adoption);
                return RedirectToAction(nameof(Index));
            }

            var dogs = await _dogService.GetAllDogsAsync();
            var cats = await _catService.GetAllCatsAsync();

            viewModel.Dogs = dogs.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();
            viewModel.Cats = cats.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();

            return View(viewModel);
        }

        // GET: Adoptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null)
            {
                return NotFound();
            }

            var dogs = await _dogService.GetAllDogsAsync();
            var cats = await _catService.GetAllCatsAsync();

            var viewModel = new AdoptionViewModel
            {
                Id = adoption.Id,
                AdoptionDate = adoption.AdoptionDate,
                FreeTransportation = adoption.FreeTransportation,
                AdoptionStory = adoption.AdoptionStory,
                DogId = adoption.DogId,
                CatId = adoption.CatId,
                Dogs = dogs.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList(),
                Cats = cats.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };

            return View(viewModel);
        }

        // POST: Adoptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdoptionViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var adoption = new Adoption
                {
                    Id = viewModel.Id,
                    AdoptionDate = viewModel.AdoptionDate,
                    FreeTransportation = viewModel.FreeTransportation,
                    AdoptionStory = viewModel.AdoptionStory,
                    DogId = viewModel.DogId,
                    CatId = viewModel.CatId
                };

                try
                {
                    await _adoptionService.UpdateAdoptionAsync(adoption);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            var dogs = await _dogService.GetAllDogsAsync();
            var cats = await _catService.GetAllCatsAsync();

            viewModel.Dogs = dogs.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();
            viewModel.Cats = cats.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();

            return View(viewModel);
        }

        // GET: Adoptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoption = await _adoptionService.GetAdoptionByIdAsync(id.Value);
            if (adoption == null)
            {
                return NotFound();
            }

            return View(adoption);
        }

        // POST: Adoptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adoptionService.DeleteAdoptionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
