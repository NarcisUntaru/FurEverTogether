using FurEver_Together.DataModels;
using FurEver_Together.Services.Interfaces;
using System.Threading.Tasks;

namespace FurEverTogether.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IPetService _petService;
        private readonly IUserProfileService _userProfileService;

        // Constructor injection of dependencies
        public MatchingService(IPetService petService, IUserProfileService userProfileService)
        {
            _petService = petService;
            _userProfileService = userProfileService;
        }

        public double CalculateMatchPercentage(PersonalityProfile adopter, PersonalityProfile pet)
        {
            // Euclidean distance

            var adopterVector = adopter.ToVector();
            var petVector = pet.ToVector();

            double distance = 0;
            for (int i = 0; i < adopterVector.Count; i++)
            {
                distance += Math.Pow(adopterVector[i] - petVector[i], 2);
            }

            double maxDistance = Math.Pow(2, 2) * adopterVector.Count;
            double similarity = 1 - (Math.Sqrt(distance) / Math.Sqrt(maxDistance));

            return similarity * 100;
        }

        public async Task<double> GetMatchPercentageForPetAndUserAsync(int petId, string userId)
        {
            var pet = await _petService.GetPetByIdAsync(petId);
            if (pet == null || pet.Personality == null)
                return 0;

            var adopterProfile = await _userProfileService.GetPersonalityProfileByUserIdAsync(userId);
            if (adopterProfile == null)
                return 0;

            return CalculateMatchPercentage(adopterProfile, pet.Personality);
        }
    }
}
