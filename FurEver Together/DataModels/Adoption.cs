using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurEver_Together.DataModels
{
    public partial class Adoption
    {
        public int Id { get; set; }
        public DateTime AdoptionDate { get; set; }
        public bool FreeTransportation { get; set; }
        public string AdoptionStory { get; set; }

        // Navigation properties

        public User User { get; set; }

        public int? CatId { get; set; }
        public Cat Cat { get; set; }

        public int? DogId { get; set; }
        public Dog Dog { get; set; }
    }
}