using FurEver_Together.DataModels;
using System.Threading.Tasks;

namespace FurEver_Together.Services.Interfaces
{
    public interface IAdoptionService
    {
        Task<Adoption> GetAdoptionByIdAsync(int id);
        Task<Adoption> GetAdoptionByPetIdAsync(int petId);
        Task<IEnumerable<Adoption>> GetAllAdoptionsAsync();
        Task AddAdoptionAsync(Adoption adoption);
        Task UpdateAdoptionAsync(Adoption adoption);
        Task DeleteAdoptionAsync(int id);
        Task<Adoption?> GetAdoptionByPetAndUserAsync(int petId, string userId);
        Task<List<Adoption>> GetAdoptionsByUserIdAsync(string userId);
    }
}
