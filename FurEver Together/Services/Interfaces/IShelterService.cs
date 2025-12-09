using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IShelterService
    {
        Task<IEnumerable<Shelter>> GetAllSheltersAsync();
        Task<Shelter?> GetShelterByIdAsync(int id);
        Task<Shelter?> GetShelterWithPetsAsync(int id);
        Task AddShelterAsync(Shelter shelter);
        Task UpdateShelterAsync(Shelter shelter);
        Task DeleteShelterAsync(int id);
    }
}