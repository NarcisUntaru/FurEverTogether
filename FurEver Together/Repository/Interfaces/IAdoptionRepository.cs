using FurEver_Together.DataModels;

namespace FurEver_Together.Repository.Interfaces;

public interface IAdoptionRepository : IGenericRepository<Adoption>
{
    Task<Adoption> FindByIdAsync(int id);
    Task<Adoption> GetAdoptionByPetIdAsync(int petId);
    Task<Adoption?> GetAdoptionByPetAndUserAsync(int petId, string userId);
}