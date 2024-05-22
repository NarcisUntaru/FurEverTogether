using Microsoft.AspNetCore.Identity;

namespace FurEver_Together.DataModels
{
    public partial class User : IdentityUser
    {

        // Navigation properties
        public int? AdoptionId { get; set; }

        public Adoption Adoption { get; set; }

        public ICollection<ContactUs> ContactMessages { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
