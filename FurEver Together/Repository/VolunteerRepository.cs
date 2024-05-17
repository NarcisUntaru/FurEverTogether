using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class VolunteerRepository : GenericRepository<Volunteer>, IVolunteerRepository
    {
        public VolunteerRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}