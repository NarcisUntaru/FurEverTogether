using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}