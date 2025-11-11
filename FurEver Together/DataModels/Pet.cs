using FurEver_Together.Enums;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public class Pet
    {
        [JsonRequired]
        public int PetId { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public PetType Type { get; set; }
        [JsonRequired]
        public string Breed { get; set; }
        [JsonRequired]
        public int Age { get; set; }
        [JsonRequired]
        public string Gender { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public DateTime ArrivalDate { get; set; }
        [JsonRequired]
        public bool IsAdopted { get; set; }
        [JsonRequired]
        public string PictureUrl { get; set; }
        [JsonRequired]
        public Adoption Adoption { get; set; }
        [JsonRequired]
        public PersonalityProfile Personality { get; set; }
    }
}
