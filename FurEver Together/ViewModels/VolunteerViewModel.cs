using FurEver_Together.Enums;
using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.ViewModels
{
    public class VolunteerViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public bool TransportationAvailable { get; set; }
        [Required]
        public bool PreviousExperience { get; set; }
        [Required]
        public int HoursPerWeek { get; set; }
        [Required]
        public bool AgreementToTerms { get; set; }
        [Required]
        public bool IsOver18 { get; set; }
        [Required]
        public DateTime? RespondDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public ApplicationStatus Status { get; set; }

    }
}