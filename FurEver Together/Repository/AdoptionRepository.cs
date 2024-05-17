using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class AdoptionRepository : GenericRepository<Adoption>, IAdoptionRepository
    {
        public AdoptionRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}