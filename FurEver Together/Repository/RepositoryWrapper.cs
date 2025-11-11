using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurEver_Together.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly FurEverTogetherDbContext _furEverTogetherDbContext;
    private IAdoptionRepository? _adoptionRepository;
    private IContactUsRepository? _contactUsRepository;
    private IPetRepository? _petRepository;
    private IUserRepository? _userRepository;
    private IVolunteerRepository? _volunteerRepository;

    public IAdoptionRepository AdoptionRepository =>
        _adoptionRepository ??= new AdoptionRepository(_furEverTogetherDbContext);

    public IContactUsRepository ContactUsRepository =>
        _contactUsRepository ??= new ContactUsRepository(_furEverTogetherDbContext);

    public IPetRepository PetRepository =>
        _petRepository ??= new PetRepository(_furEverTogetherDbContext);

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_furEverTogetherDbContext);

    public IVolunteerRepository VolunteerRepository =>
        _volunteerRepository ??= new VolunteerRepository(_furEverTogetherDbContext);

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