using System.Threading.Tasks;
using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly FurEverTogetherDbContext _furEverTogetherDbContext;

        public UserProfileService(FurEverTogetherDbContext dbContext)
        {
            _furEverTogetherDbContext = dbContext;
        }

        public async Task<PersonalityProfile?> GetPersonalityProfileByUserIdAsync(string userId)
        {
            // Assuming your ApplicationUser entity has a navigation property to PersonalityProfile
            var userWithProfile = await _furEverTogetherDbContext.Users
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return userWithProfile?.Preferences;
        }
    }
}