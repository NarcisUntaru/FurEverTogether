using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository
{
    public class ContactUsRepository : GenericRepository<ContactUs>, IContactUsRepository
    {
        public ContactUsRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}