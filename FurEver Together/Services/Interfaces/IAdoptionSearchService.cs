using FurEver_Together.DataModels;
using System.Collections.Generic;

namespace FurEver_Together.Services.Interfaces
{
    public interface IAdoptionSearchService
    {
        List<Adoption> SearchAdoptions(string animalType = null, string breed = null, string age = null, string gender = null);
    }
}