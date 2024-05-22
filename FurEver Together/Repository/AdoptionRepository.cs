using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class AdoptionRepository : GenericRepository<Adoption>, IAdoptionRepository
    {
        public AdoptionRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
            
        }
        public async Task<Adoption> FindByIdAsync(int id)
        {
            return await dbContext.Adoptions.FindAsync(id);
        }
    }
}