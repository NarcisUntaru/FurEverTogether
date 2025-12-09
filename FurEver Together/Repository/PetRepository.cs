using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
           
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            return await dbContext.Pets
                .Include(p => p.Adoption)
                .Include(p => p.Shelter)
                .ToListAsync();
        }

        public override async Task<Pet?> GetByIdAsync(int id)
        {
            return await dbContext.Pets
                .Include(p => p.Adoption)
                .Include(p => p.Shelter)
                .Include(p => p.Personality)
                .FirstOrDefaultAsync(p => p.PetId == id);
        }
    }
}