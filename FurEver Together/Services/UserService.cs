using FurEver_Together.DataModels;
using FurEver_Together.Repository;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;

namespace FurEver_Together.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
    }
}