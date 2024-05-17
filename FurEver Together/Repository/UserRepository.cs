using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}