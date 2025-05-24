using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<Pet> GetPetByIdAsync(int id);
        Task AddPetAsync(Pet pet);
        Task UpdatePetAsync(Pet pet);
        Task DeletePetAsync(int id);
    }
}
