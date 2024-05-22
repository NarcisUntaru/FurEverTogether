using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FurEver_Together.Services
{
    public class CatService : ICatService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CatService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public IEnumerable<Cat> GetAllCats()
        {
            return _repositoryWrapper.CatRepository.GetAll();
        }
        public async Task<IEnumerable<Cat>> GetAllCatsAsync()
        {
            return await _repositoryWrapper.CatRepository.GetAllAsync();
        }

        public async Task<Cat> GetCatByIdAsync(int id)
        {
            return await _repositoryWrapper.CatRepository.GetByIdAsync(id);
        }

        public async Task AddCatAsync(Cat cat)
        {
            _repositoryWrapper.CatRepository.Add(cat);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateCatAsync(Cat cat)
        {
            _repositoryWrapper.CatRepository.Update(cat);
            await _repositoryWrapper.SaveAsync();
        }



        public async Task DeleteCatAsync(int id)
        {
            var cat = await _repositoryWrapper.CatRepository.GetByIdAsync(id);
            if (cat != null)
            {
                _repositoryWrapper.CatRepository.Delete(cat);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}
