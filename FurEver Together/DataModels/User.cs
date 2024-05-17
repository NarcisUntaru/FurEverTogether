namespace FurEver_Together.DataModels
{
    public partial class User : BaseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        // Navigation properties
        public ICollection<Adoption> Adoptions { get; set; }

        public ICollection<ContactUs> ContactMessages { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}