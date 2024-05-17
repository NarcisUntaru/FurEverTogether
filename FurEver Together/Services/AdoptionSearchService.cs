using FurEver_Together.DataModels;
using FurEver_Together.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class AdoptionSearchService : IAdoptionSearchService
    {
        private readonly IAdoptionRepository _adoptionRepository;

        public AdoptionSearchService(IAdoptionRepository adoptionRepository)
        {
            _adoptionRepository = adoptionRepository;
        }

        public List<Adoption> SearchAdoptions(string animalType = null, string breed = null, string age = null, string gender = null)
        {
            IQueryable<Adoption> query = (IQueryable<Adoption>)_adoptionRepository.GetAll();

            if (!string.IsNullOrEmpty(animalType))
                query = query.Where(a => a.AnimalType.ToLower() == animalType.ToLower());

            if (!string.IsNullOrEmpty(breed))
                query = query.Where(a => a.Breed.ToLower().Contains(breed.ToLower()));

            if (!string.IsNullOrEmpty(age))
                query = query.Where(a => a.Age.ToLower() == age.ToLower());

            if (!string.IsNullOrEmpty(gender))
                query = query.Where(a => a.Gender.ToLower() == gender.ToLower());

            return query.ToList();
        }
    }
}