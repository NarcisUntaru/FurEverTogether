using FurEver_Together.DataModels;
using FurEver_Together.Repository;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class DogService : IDogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public DogService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<Dog>> GetAllDogsAsync()
        {
            return await _repositoryWrapper.DogRepository.GetAllAsync();
        }
        public IEnumerable<Dog> GetAllDogs()
        {
            return _repositoryWrapper.DogRepository.GetAll();
        }
        public async Task<Dog> GetDogByIdAsync(int id)
        {
            return await _repositoryWrapper.DogRepository.GetByIdAsync(id);
        }

        public async Task AddDogAsync(Dog dog)
        {
            _repositoryWrapper.DogRepository.Add(dog);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateDogAsync(Dog dog)
        {
            _repositoryWrapper.DogRepository.Update(dog);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteDogAsync(int id)
        {
            var dog = await _repositoryWrapper.DogRepository.GetByIdAsync(id);
            if (dog != null)
            {
                _repositoryWrapper.DogRepository.Delete(dog);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}