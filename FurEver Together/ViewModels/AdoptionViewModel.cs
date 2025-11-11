using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FurEver_Together.ViewModels
{
    public class AdoptionViewModel
    {
        [JsonRequired]
        public int Id { get; set; }
        public DateTime? AdoptionDate { get; set; }


        // Navigation properties

        [JsonRequired]
        public int PetId { get; set; }
        [JsonRequired]
        public List<SelectListItem> Pets { get; set; }
        [JsonRequired]
        public Pet Pet { get; set; }
    }
}