using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public partial class User : IdentityUser
    {

        // Navigation properties

        public ICollection<Adoption> Adoptions { get; set; } = new List<Adoption>();
        
        public ICollection<ContactUs>? ContactMessages { get; set; }
        public Volunteer? Volunteer { get; set; }
        public PersonalityProfile? Preferences { get; set; }
    }
}
