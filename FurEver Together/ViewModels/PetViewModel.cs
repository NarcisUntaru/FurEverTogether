using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FurEver_Together.ViewModels
{
    public class PetViewModel
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
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        [JsonRequired]
        public double MatchPercentage { get; set; }

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
    }
}
