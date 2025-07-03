using FurEver_Together.DataModels;

namespace FurEver_Together.Repository.Interfaces;

public interface IPetRepository : IGenericRepository<Pet>
{
    Task<IEnumerable<Pet>> GetAllAsync();
}