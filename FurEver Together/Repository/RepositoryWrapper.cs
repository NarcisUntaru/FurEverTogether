using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

namespace FurEver_Together.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FurEverTogetherDbContext _furEverTogetherDbContext;
    private IAdoptionRepository? _adoptionRepository;
    private ICatRepository? _catRepository;
    private IContactUsRepository? _contactUsRepository;
    private IDogRepository? _dogRepository;
    private IUserRepository? _userRepository;
    private IVolunteerRepository? _volunteerRepository;

    public IAdoptionRepository AdoptionRepository
    {
        get { return _adoptionRepository ??= new AdoptionRepository(_furEverTogetherDbContext); }
    }

    public ICatRepository CatRepository
    {
        get { return _catRepository ??= new CatRepository(_furEverTogetherDbContext); }
    }

    public IContactUsRepository ContactUsRepository
    {
        get { return _contactUsRepository ??= new ContactUsRepository(_furEverTogetherDbContext); }
    }
    public IDogRepository DogRepository
    {
        get { return _dogRepository ??= new DogRepository(_furEverTogetherDbContext); }
    }

    public IUserRepository UserRepository
    {
        get { return _userRepository ??= new UserRepository(_furEverTogetherDbContext); }
    }
    public IVolunteerRepository VolunteerRepository
    {
        get { return _volunteerRepository ??= new VolunteerRepository(_furEverTogetherDbContext); }
    }

    public RepositoryWrapper(FurEverTogetherDbContext furEverTogetherDbContext)
    {
        _furEverTogetherDbContext = furEverTogetherDbContext;
    }

    public void Save()
    {
        _furEverTogetherDbContext.SaveChanges();
    }
}
