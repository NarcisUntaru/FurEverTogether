using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.ViewModels;
using FurEver_Together.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FurEver_Together.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogService _dogService;

        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }

        // GET: Dogs
        public async Task<IActionResult> Index()
        {
            var dogs = await _dogService.GetAllDogsAsync();
            return View(dogs);
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _dogService.GetDogByIdAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Dogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DogViewModel dogViewModel)
        {
            if (ModelState.IsValid)
            {
                var dog = new Dog
                {
                    Name = dogViewModel.Name,
                    Breed = dogViewModel.Breed,
                    Age = dogViewModel.Age,
                    Gender = dogViewModel.Gender,
                    ImageURL = dogViewModel.ImageURL,
                    Description = dogViewModel.Description,
                    Size = dogViewModel.Size,
                    Trained = dogViewModel.Trained
                };

                await _dogService.AddDogAsync(dog);
                return RedirectToAction(nameof(Index));
            }
            return View(dogViewModel);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _dogService.GetDogByIdAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }

            var viewModel = new DogViewModel
            {
                Id = dog.Id,
                Name = dog.Name,
                Breed = dog.Breed,
                Age = dog.Age,
                Gender = dog.Gender,
                ImageURL = dog.ImageURL,
                Description = dog.Description,
                Size = dog.Size,
                Trained = dog.Trained
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DogViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dog = await _dogService.GetDogByIdAsync(id);
                if (dog == null)
                {
                    return NotFound();
                }

                // Map ViewModel to Model
                dog.Name = viewModel.Name;
                dog.Breed = viewModel.Breed;
                dog.Age = viewModel.Age;
                dog.Gender = viewModel.Gender;
                dog.ImageURL = viewModel.ImageURL;
                dog.Description = viewModel.Description;
                dog.Size = viewModel.Size;
                dog.Trained = viewModel.Trained;

                try
                {
                    await _dogService.UpdateDogAsync(dog);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DogExists(dog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Dogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _dogService.GetDogByIdAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dogService.DeleteDogAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DogExists(int id)
        {
            var dog = await _dogService.GetDogByIdAsync(id);
            return dog != null;
        }
    }
}
