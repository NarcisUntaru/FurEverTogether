using FurEver_Together.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurEver_Together.DataModels
{
    public partial class Adoption
    {
        public int Id { get; set; }
        public DateTime? AdoptionDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        // Navigation properties

        public User User { get; set; }
        public string? UserId { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }

    }
}