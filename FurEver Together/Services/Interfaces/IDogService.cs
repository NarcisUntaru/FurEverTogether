using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IDogService
    {
        Task<IEnumerable<Dog>> GetAllDogsAsync();
        IEnumerable<Dog> GetAllDogs();
        Task<Dog> GetDogByIdAsync(int id);
        Task AddDogAsync(Dog dog);
        Task UpdateDogAsync(Dog dog);
        Task DeleteDogAsync(int id);

    }
}
