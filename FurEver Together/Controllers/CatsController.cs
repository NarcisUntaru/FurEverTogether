using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FurEver_Together.Services.Interfaces;
using FurEver_Together.DataModels;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FurEver_Together.Controllers
{
    public class CatsController : Controller
    {
        private readonly ICatService _catService;

        public CatsController(ICatService catService)
        {
            _catService = catService;
        }
        
        // GET: Cats
        public async Task<IActionResult> Index()
        {
            var cats = await _catService.GetAllCatsAsync();
            return View(cats);
        }

        // GET: Cats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _catService.GetCatByIdAsync(id.Value);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Cats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatViewModel catViewModel)
        {
            if (ModelState.IsValid)
            {
                var cat = new Cat
                {
                    Name = catViewModel.Name,
                    Breed = catViewModel.Breed,
                    Age = catViewModel.Age,
                    Gender = catViewModel.Gender,
                    ImageURL = catViewModel.ImageURL,
                    Description = catViewModel.Description,
                    Declawed = catViewModel.Declawed,
                    Vaccinated = catViewModel.Vaccinated,
                };

                await _catService.AddCatAsync(cat);
                return RedirectToAction(nameof(Index));
            }
            return View(catViewModel);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _catService.GetCatByIdAsync(id.Value);
            if (cat == null)
            {
                return NotFound();
            }

            var viewModel = new CatViewModel
            {
                Id = cat.Id,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age,
                Gender = cat.Gender,
                ImageURL = cat.ImageURL,
                Description = cat.Description,
                Declawed = cat.Declawed,
                Vaccinated = cat.Vaccinated
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cat = await _catService.GetCatByIdAsync(id);
                if (cat == null)
                {
                    return NotFound();
                }

                cat.Name = viewModel.Name;
                cat.Breed = viewModel.Breed;
                cat.Age = viewModel.Age;
                cat.Gender = viewModel.Gender;
                cat.ImageURL = viewModel.ImageURL;
                cat.Description = viewModel.Description;
                cat.Declawed = viewModel.Declawed;
                cat.Vaccinated = viewModel.Vaccinated;

                try
                {
                    await _catService.UpdateCatAsync(cat);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CatExists(cat.Id))
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
        // GET: Cats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await _catService.GetCatByIdAsync(id.Value);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _catService.DeleteCatAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CatExists(int id)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            return cat != null;
        }
    }
}
