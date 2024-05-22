using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public VolunteerService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public void Add(Volunteer volunteer)
        {
            _repositoryWrapper.VolunteerRepository.Add(volunteer);
            _repositoryWrapper.Save();
        }
    }
}