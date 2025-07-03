using System.Threading.Tasks;
using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<PersonalityProfile?> GetPersonalityProfileByUserIdAsync(string userId);
    }
}
