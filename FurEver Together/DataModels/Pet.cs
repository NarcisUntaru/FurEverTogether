using FurEver_Together.Enums;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public class Pet
    {
        [JsonRequired]
        public int PetId { get; set; }
        public string? Name { get; set; }
        [JsonRequired]
        public PetType Type { get; set; }
        public string? Breed { get; set; }
        [JsonRequired]
        public int Age { get; set; }
        public string? Gender { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public DateTime ArrivalDate { get; set; }
        [JsonRequired]
        public bool IsAdopted { get; set; }
        public string? PictureUrl { get; set; }
        public Adoption? Adoption { get; set; }
        public PersonalityProfile? Personality { get; set; }
    }
}
