using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Services
{
    public class AdoptionService : IAdoptionService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AdoptionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<Adoption> GetAdoptionByPetIdAsync(int petId)
        {
            return await _repositoryWrapper.AdoptionRepository.GetAdoptionByPetIdAsync(petId);
        }
        public async Task<Adoption> GetAdoptionByIdAsync(int id)
        {
            return await _repositoryWrapper.AdoptionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Adoption>> GetAllAdoptionsAsync()
        {
            return await _repositoryWrapper.AdoptionRepository.GetAllAsync();
        }
        public async Task AddAdoptionAsync(Adoption adoption)
        {
            _repositoryWrapper.AdoptionRepository.Add(adoption);
            await _repositoryWrapper.SaveAsync();
        }
        public async Task UpdateAdoptionAsync(Adoption adoption)
        {
            _repositoryWrapper.AdoptionRepository.Update(adoption);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteAdoptionAsync(int id)
        {
            var adoption = await _repositoryWrapper.AdoptionRepository.GetByIdAsync(id);
            if (adoption != null)
            {
                _repositoryWrapper.AdoptionRepository.Delete(adoption);
                await _repositoryWrapper.SaveAsync();
            }
        }
        public async Task<Adoption?> GetAdoptionByPetAndUserAsync(int petId, string userId)
        {
            return await _repositoryWrapper.AdoptionRepository.GetAdoptionByPetAndUserAsync(petId, userId);
        }


    }
}