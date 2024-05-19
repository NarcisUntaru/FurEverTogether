namespace FurEver_Together.DataModels
{
    public partial class Cat
    {
        public int Id { get; set; }
        public bool Declawed { get; set; }
        public bool Vaccinated { get; set; }

        // Navigation property
        public Adoption Adoption { get; set; }
    }
}