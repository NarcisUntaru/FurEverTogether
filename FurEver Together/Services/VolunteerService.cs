using FurEver_Together.DataModels;
using FurEver_Together.Repository;
using FurEver_Together.Repository.Interfaces;
using FurEver_Together.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public Volunteer GetById(int id)
        {
            // Assuming VolunteerRepository has GetAll or similar
            return _repositoryWrapper.VolunteerRepository.GetAll()
                .FirstOrDefault(v => v.Id == id);
        }
        public async Task<Volunteer> GetByIdAsync(int id)
        {
            return await _repositoryWrapper.VolunteerRepository.GetByIdAsync(id);
        }

        public void Delete(int id)
        {
            var volunteer = GetById(id);
            if (volunteer != null)
            {
                _repositoryWrapper.VolunteerRepository.Delete(volunteer);
                _repositoryWrapper.Save();
            }
        }
        public bool HasSubmitted(string userId)
        {
            return _repositoryWrapper.VolunteerRepository.GetAll().Any(v => v.UserId == userId);
        }
        public void Update(Volunteer volunteer)
        {
            _repositoryWrapper.VolunteerRepository.Update(volunteer);
            _repositoryWrapper.Save();
        }
        public async Task UpdateAsync(Volunteer volunteer)
        {
            _repositoryWrapper.VolunteerRepository.Update(volunteer);
            await _repositoryWrapper.SaveAsync();
        }
    }
}
