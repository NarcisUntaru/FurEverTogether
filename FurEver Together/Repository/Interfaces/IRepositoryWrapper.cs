namespace FurEver_Together.Repository.Interfaces;

public interface IRepositoryWrapper
{
    IAdoptionRepository AdoptionRepository { get; }
    ICatRepository CatRepository { get; }
    IContactUsRepository ContactUsRepository { get; }
    IDogRepository DogRepository { get; }
    IUserRepository UserRepository { get; }
    IVolunteerRepository VolunteerRepository { get; }

    void Save();
}