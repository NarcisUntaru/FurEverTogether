namespace FurEver_Together.DataModels
{
    public partial class Adoption : BaseModel
    {
        public string AnimalType { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public int UserId { get; set; }

        public User User { get; set; }

        public int? CatId { get; set; }
        public Cat Cat { get; set; }

        public int? DogId { get; set; }
        public Dog Dog { get; set; }
    }
}