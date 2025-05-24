using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class PetService : IPetService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PetService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
           return await _repositoryWrapper.PetRepository.GetAllAsync();
        }
        public async Task<Pet> GetPetByIdAsync(int id)
        {
            return await _repositoryWrapper.PetRepository.GetByIdAsync(id);
        }
        public async Task AddPetAsync(Pet pet)
        {
            _repositoryWrapper.PetRepository.Add(pet);
            await _repositoryWrapper.SaveAsync();
        }
        public async Task UpdatePetAsync(Pet pet)
        {
            _repositoryWrapper.PetRepository.Update(pet);
            await _repositoryWrapper.SaveAsync();
        }
        public async Task DeletePetAsync(int id)
        {
            var pet = await _repositoryWrapper.PetRepository.GetByIdAsync(id);
            if (pet != null)
            {
                _repositoryWrapper.PetRepository.Delete(pet);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}