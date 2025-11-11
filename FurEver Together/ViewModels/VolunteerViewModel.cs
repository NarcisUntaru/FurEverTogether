using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FurEver_Together.ViewModels
{
    public class VolunteerViewModel
    {
        [JsonRequired]
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
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
        [JsonRequired]
        public DateTime? RespondDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public ApplicationStatus? Status { get; set; }

    }
}