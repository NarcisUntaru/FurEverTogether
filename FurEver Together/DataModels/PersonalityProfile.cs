using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public class PersonalityProfile
    {
        [JsonRequired]
        public EnergyLevel EnergyLevel { get; set; }
        [JsonRequired]
        public Sociability Sociability { get; set; }
        [JsonRequired]
        public AffectionLevel AffectionLevel { get; set; }
        [JsonRequired]
        public Trainability Trainability { get; set; }
        [JsonRequired]
        public Playfulness Playfulness { get; set; }
        [JsonRequired]
        public AggressionLevel AggressionLevel { get; set; }
        [JsonRequired]
        public NoiseLevel NoiseLevel { get; set; }
        [JsonRequired]
        public Question GoodWithKids { get; set; }
        [JsonRequired]
        public Question GoodWithOtherPets { get; set; }
        [JsonRequired]
        public Adaptability Adaptability { get; set; }
        [JsonRequired]
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
