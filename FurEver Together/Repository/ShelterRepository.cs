using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository
{
    public class ShelterRepository : GenericRepository<Shelter>, IShelterRepository
    {
        public ShelterRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Shelter>> GetAllWithPetsAsync()
        {
            return await dbContext.Shelters
                .Include(s => s.Pets)
                .ToListAsync();
        }

        public async Task<Shelter?> GetByIdWithPetsAsync(int id)
        {
            return await dbContext.Shelters
                .Include(s => s.Pets)
                .FirstOrDefaultAsync(s => s.ShelterId == id);
        }
    }
}