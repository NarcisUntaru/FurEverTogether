using Microsoft.AspNetCore.Identity;

namespace FurEver_Together.DataModels
{
    public partial class User : IdentityUser
    {

        // Navigation properties
        public ICollection<Adoption> Adoptions { get; set; }

        public ICollection<ContactUs> ContactMessages { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
