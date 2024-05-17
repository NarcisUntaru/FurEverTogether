using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class CatRepository : GenericRepository<Cat>, ICatRepository
    {
        public CatRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}