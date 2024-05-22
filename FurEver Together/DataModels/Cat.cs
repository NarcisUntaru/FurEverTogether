using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.DataModels
{
    public partial class Cat
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public bool Declawed { get; set; }
        public bool Vaccinated { get; set; }

        // Navigation property
        public Adoption Adoption { get; set; }
    }
}