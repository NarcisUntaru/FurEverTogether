using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository
{
    public class VolunteerRepository : GenericRepository<Volunteer>, IVolunteerRepository
    {
        public VolunteerRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
            void Update(Volunteer volunteer)
            {
                dbContext.Volunteers.Update(volunteer);
            }
        }
    }
}