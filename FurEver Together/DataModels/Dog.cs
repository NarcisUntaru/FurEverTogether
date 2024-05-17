namespace FurEver_Together.DataModels
{
    public partial class Dog : BaseModel
    {
        public string Size { get; set; }
        public bool Trained { get; set; }

        // Navigation property
        public Adoption Adoption { get; set; }
    }
}