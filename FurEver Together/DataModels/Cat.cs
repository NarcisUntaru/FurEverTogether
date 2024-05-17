namespace FurEver_Together.DataModels
{
    public partial class Cat : BaseModel
    {
        public bool Declawed { get; set; }
        public bool Vaccinated { get; set; }

        // Navigation property
        public Adoption Adoption { get; set; }
    }
}