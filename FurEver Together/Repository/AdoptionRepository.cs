using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository
{
    public class AdoptionRepository : GenericRepository<Adoption>, IAdoptionRepository
    {
        public AdoptionRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
            
        }
        public async Task<Adoption?> GetByIdAsync(int id)
        {
            return await dbContext.Adoptions
                .Include(a => a.Pet)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Adoption> FindByIdAsync(int id)
        {
            return await dbContext.Adoptions.FindAsync(id);
        }
        public async Task<Adoption> GetAdoptionByPetIdAsync(int petId)
        {
            return await dbContext.Adoptions.FirstOrDefaultAsync(a => a.PetId == petId);
        }
        public async Task<Adoption?> GetAdoptionByPetAndUserAsync(int petId, string userId)
        {
            return await dbContext.Adoptions
                .FirstOrDefaultAsync(a => a.PetId == petId && a.UserId == userId);
        }
    }
}