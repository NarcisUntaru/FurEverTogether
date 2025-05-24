using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.ViewModels
{
    public class AdoptionViewModel
    {
        public int Id { get; set; }
        public DateTime? AdoptionDate { get; set; }


        // Navigation properties

        public int PetId { get; set; }
        public List<SelectListItem> Pets { get; set; }
        public Pet Pet { get; set; }
    }
}