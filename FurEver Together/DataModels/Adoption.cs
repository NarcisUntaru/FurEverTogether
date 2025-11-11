using FurEver_Together.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace FurEver_Together.DataModels
{
    public partial class Adoption
    {
        [JsonRequired]
        public int Id { get; set; }
        public DateTime? AdoptionDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        // Navigation properties
        [JsonRequired]
        public User User { get; set; }
        public string? UserId { get; set; }
        [JsonRequired]
        public int PetId { get; set; }
        [JsonRequired]
        public Pet Pet { get; set; }

    }
}