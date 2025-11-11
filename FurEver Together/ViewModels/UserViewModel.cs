using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace FurEver_Together.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [JsonRequired]
        public string Username { get; set; }
        [JsonRequired]
        public string Email { get; set; }
        [JsonRequired]
        public string Password { get; set; }
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonRequired]
        public string LastName { get; set; }
        [JsonRequired]
        public string PhoneNumber { get; set; }
    }
}