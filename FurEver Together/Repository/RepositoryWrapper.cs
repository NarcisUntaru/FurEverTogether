using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FurEverTogetherDbContext _furEverTogetherDbContext;
    private IAdoptionRepository? _adoptionRepository;
    //private ICatRepository? _catRepository;
    private IContactUsRepository? _contactUsRepository;
    private IPetRepository? _petRepository;
    private IUserRepository? _userRepository;
    private IVolunteerRepository? _volunteerRepository;

    public IAdoptionRepository AdoptionRepository
    {
        get
        {
            if (_adoptionRepository == null)
            {
                _adoptionRepository = new AdoptionRepository(_furEverTogetherDbContext);
            }

            return _adoptionRepository;
        }
    }

    //public ICatRepository CatRepository
    //{
    //    get
    //    {
    //        if (_catRepository == null)
    //        {
    //            _catRepository = new CatRepository(_furEverTogetherDbContext);
    //        }

    //        return _catRepository;
    //    }
    //}

    public IContactUsRepository ContactUsRepository
    {
        get
        {
            if (_contactUsRepository == null)
            {
                _contactUsRepository = new ContactUsRepository(_furEverTogetherDbContext);
            }

            return _contactUsRepository;
        }
    }

    public IPetRepository PetRepository
    {
        get
        {
            if (_petRepository == null)
            {
                _petRepository = new PetRepository(_furEverTogetherDbContext);
            }

            return _petRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_furEverTogetherDbContext);
            }

            return _userRepository;
        }
    }
    public IVolunteerRepository VolunteerRepository
    {
        get
        {
            if (_volunteerRepository == null)
            {
                _volunteerRepository = new VolunteerRepository(_furEverTogetherDbContext);
            }

            return _volunteerRepository;
        }
    }

    public RepositoryWrapper(FurEverTogetherDbContext furEverTogetherDbContext)
    {
        _furEverTogetherDbContext = furEverTogetherDbContext;
    }

    public void Save()
    {
        _furEverTogetherDbContext.SaveChanges();
    }
    public async Task SaveAsync()
    {
        await _furEverTogetherDbContext.SaveChangesAsync();
    }
    
}
