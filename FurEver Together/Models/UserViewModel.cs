using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Identity;

namespace FurEver_Together.Models
{
    public class UserViewModel : IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}