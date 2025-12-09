using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository _shelterRepository;

        public ShelterService(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        public async Task<IEnumerable<Shelter>> GetAllSheltersAsync()
        {
            return await _shelterRepository.GetAllWithPetsAsync();
        }

        public async Task<Shelter?> GetShelterByIdAsync(int id)
        {
            return await _shelterRepository.GetByIdAsync(id);
        }

        public async Task<Shelter?> GetShelterWithPetsAsync(int id)
        {
            return await _shelterRepository.GetByIdWithPetsAsync(id);
        }

        public async Task AddShelterAsync(Shelter shelter)
        {
            _shelterRepository.Add(shelter);
            await _shelterRepository.SaveAsync();
        }

        public async Task UpdateShelterAsync(Shelter shelter)
        {
            _shelterRepository.Update(shelter);
            await _shelterRepository.SaveAsync();
        }

        public async Task DeleteShelterAsync(int id)
        {
            var shelter = await _shelterRepository.GetByIdAsync(id);
            if (shelter != null)
            {
                _shelterRepository.Delete(shelter);
                await _shelterRepository.SaveAsync();
            }
        }
    }
}