using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface ICatService
    {

        Task<IEnumerable<Cat>> GetAllCatsAsync();
        IEnumerable<Cat> GetAllCats();
        Task<Cat> GetCatByIdAsync(int id);
        Task AddCatAsync(Cat cat);
        Task UpdateCatAsync(Cat cat);
        Task DeleteCatAsync(int id);
    }
}
