using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public EnergyLevel EnergyLevel { get; set; }
        [Required]
        public Sociability Sociability { get; set; }
        [Required]
        public AffectionLevel AffectionLevel { get; set; }
        [Required]
        public Trainability Trainability { get; set; }
        [Required]
        public Playfulness Playfulness { get; set; }
        [Required]
        public AggressionLevel AggressionLevel { get; set; }
        [Required]
        public NoiseLevel NoiseLevel { get; set; }
        [Required]
        public Question GoodWithKids { get; set; }
        [Required]
        public Question GoodWithOtherPets { get; set; }
        [Required]
        public Adaptability Adaptability { get; set; }
        [Required]
        public AnxietyLevel AnxietyLevel { get; set; }
    }
}
