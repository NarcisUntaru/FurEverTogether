using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.DataModels
{
    public class PersonalityProfile
    {
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

        public List<int> ToVector()
        {
            return new List<int>
            {
                (int)EnergyLevel,
                (int)Sociability,
                (int)AffectionLevel,
                (int)Trainability,
                (int)Playfulness,
                (int)AggressionLevel,
                (int)NoiseLevel,
                (int)Adaptability,
                (int)AnxietyLevel
            };
        }
    }
}
