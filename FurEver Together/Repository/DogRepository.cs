using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class DogRepository : GenericRepository<Dog>, IDogRepository
    {
        public DogRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}