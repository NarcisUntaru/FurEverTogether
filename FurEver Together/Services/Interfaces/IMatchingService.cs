using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IMatchingService
    {
        double CalculateMatchPercentage(PersonalityProfile adopter, PersonalityProfile pet);
        Task<double> GetMatchPercentageForPetAndUserAsync(int petId, string userId);
    }
}
