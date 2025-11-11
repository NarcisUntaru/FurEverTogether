using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public partial class ContactUs
    {
        public int Id { get; set; }
        public string? Message { get; set; }

        // Navigation properties
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}