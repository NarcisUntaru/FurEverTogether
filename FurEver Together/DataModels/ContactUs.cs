using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public partial class ContactUs
    {
        public int Id { get; set; }
        [JsonRequired]
        public string Message { get; set; }

        // Navigation properties
        [JsonRequired]
        public string UserId { get; set; }
        [JsonRequired]
        public User User { get; set; }
    }
}