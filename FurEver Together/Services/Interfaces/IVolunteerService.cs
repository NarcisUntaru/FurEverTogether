using FurEver_Together.DataModels;

namespace FurEver_Together.Services.Interfaces
{
    public interface IVolunteerService
    {
        void Add(Volunteer Volunteer);
        Volunteer GetById(int id);
        void Delete(int id);
        void Update(Volunteer volunteer);
        bool HasSubmitted(string userId);
    }
}
