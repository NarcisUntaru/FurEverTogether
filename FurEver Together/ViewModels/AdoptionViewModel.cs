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
        public List<SelectListItem>? Pets { get; set; }
        public Pet? Pet { get; set; }
    }
}