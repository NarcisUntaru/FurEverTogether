using FurEver_Together.DataModels;
using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.Models
{
    public class AdoptionViewModel
    {
        public int Id { get; set; }
        public string AnimalType { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
    }
}