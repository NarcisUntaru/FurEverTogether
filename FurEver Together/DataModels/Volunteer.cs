using FurEver_Together.Enums;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public partial class Volunteer
    {
        [JsonRequired]
        public int Id { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        [JsonRequired]
        public bool TransportationAvailable { get; set; }
        [JsonRequired]
        public bool PreviousExperience { get; set; }
        [JsonRequired]
        public int HoursPerWeek { get; set; }
        [JsonRequired]
        public bool AgreementToTerms { get; set; }
        [JsonRequired]
        public bool IsOver18 { get; set; }
        public DateTime? RespondDate { get; set; }
        public DateTime? RequestDate { get; set; }
        [JsonRequired]
        public ApplicationStatus Status { get; set; }

        // Navigation properties
        public string UserId { get; set; }

        public User User { get; set; }
    }
}