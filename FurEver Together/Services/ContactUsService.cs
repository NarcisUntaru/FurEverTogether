using FurEver_Together.DataModels;
using FurEver_Together.Repository;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ContactUsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public void Add(ContactUs contactUs)
        {
            _repositoryWrapper.ContactUsRepository.Add(contactUs);
            _repositoryWrapper.Save();
        }
    }
}