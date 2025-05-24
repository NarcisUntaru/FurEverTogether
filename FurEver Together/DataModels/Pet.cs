using FurEver_Together.Enums;

namespace FurEver_Together.DataModels
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool IsAdopted { get; set; }
        public string PictureUrl { get; set; }

        public Adoption Adoption { get; set; }
        public PersonalityProfile Personality { get; set; }
    }
}
