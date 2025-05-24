using FurEver_Together.Enums;

namespace FurEver_Together.ViewModels
{
    public class VolunteerViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public bool TransportationAvailable { get; set; }
        public bool PreviousExperience { get; set; }
        public int HoursPerWeek { get; set; }
        public bool AgreementToTerms { get; set; }
        public bool IsOver18 { get; set; }
        public DateTime? RespondDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public ApplicationStatus Status { get; set; }

    }
}