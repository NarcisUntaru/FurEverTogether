using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class CatRepository : GenericRepository<Cat>, ICatRepository
    {
        public CatRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}