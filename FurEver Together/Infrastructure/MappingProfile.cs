using AutoMapper;
using FurEver_Together.ViewModels;
using FurEver_Together.DataModels;

namespace FurEver_Together.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Adoption, AdoptionViewModel>().ReverseMap();
            CreateMap<Pet, PetViewModel>().ReverseMap();
            CreateMap<ContactUs, ContactUsViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Volunteer, VolunteerViewModel>().ReverseMap();
        }
    }
}