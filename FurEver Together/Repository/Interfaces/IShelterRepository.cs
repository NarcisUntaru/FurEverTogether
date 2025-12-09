using FurEver_Together.DataModels;

namespace FurEver_Together.Repository.Interfaces
{
    public interface IShelterRepository : IGenericRepository<Shelter>
    {
        Task<IEnumerable<Shelter>> GetAllWithPetsAsync();
        Task<Shelter?> GetByIdWithPetsAsync(int id);
    }
}