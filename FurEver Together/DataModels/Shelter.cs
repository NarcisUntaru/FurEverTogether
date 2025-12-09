using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public class Shelter
    {
        [JsonRequired]
        public int ShelterId { get; set; }
        
        [JsonRequired]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [JsonRequired]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
        
        [JsonRequired]
        public int Capacity { get; set; }
        
        [JsonRequired]
        public double Latitude { get; set; }
        
        [JsonRequired]
        public double Longitude { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        // Navigation property
        public ICollection<Pet> Pets { get; set; } = new List<Pet>();

        [NotMapped]
        public string Location => Address;
    }
}