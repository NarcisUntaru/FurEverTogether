using FurEver_Together.Enums;

namespace FurEver_Together.ViewModels
{
    public class PetViewModel
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public double MatchPercentage { get; set; }
        public EnergyLevel EnergyLevel { get; set; }
        public Sociability Sociability { get; set; }
        public AffectionLevel AffectionLevel { get; set; }
        public Trainability Trainability { get; set; }
        public Playfulness Playfulness { get; set; }
        public AggressionLevel AggressionLevel { get; set; }
        public NoiseLevel NoiseLevel { get; set; }
        public Question GoodWithKids { get; set; }
        public Question GoodWithOtherPets { get; set; }
        public Adaptability Adaptability { get; set; }
        public AnxietyLevel AnxietyLevel { get; set; }
    }
}
