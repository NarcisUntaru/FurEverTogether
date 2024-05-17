using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class ContactUsRepository : GenericRepository<ContactUs>, IContactUsRepository
    {
        public ContactUsRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}