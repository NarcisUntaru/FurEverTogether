using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}