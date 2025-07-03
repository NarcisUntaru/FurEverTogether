using FurEver_Together.Enums;

namespace FurEver_Together.DataModels
{
    public class PersonalityProfile
    {
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
