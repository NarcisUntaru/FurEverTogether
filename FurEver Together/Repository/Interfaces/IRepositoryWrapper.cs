using FurEver_Together.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository.Interfaces;

public interface IRepositoryWrapper
{
    IAdoptionRepository AdoptionRepository { get; }
    IContactUsRepository ContactUsRepository { get; }
    IPetRepository PetRepository { get; }
    IUserRepository UserRepository { get; }
    IVolunteerRepository VolunteerRepository { get; }

    void Save();
    Task SaveAsync();
}