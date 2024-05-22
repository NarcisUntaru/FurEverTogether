using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FurEver_Together.ViewModels
{
    public class AdoptionViewModel
    {
        public int Id { get; set; }
        public DateTime AdoptionDate { get; set; }
        public bool FreeTransportation { get; set; }
        public string AdoptionStory { get; set; }
        public int? CatId { get; set; }
        public int? DogId { get; set; }
        public List<SelectListItem> Dogs { get; set; }
        public List<SelectListItem> Cats { get; set; }
        public int SelectedCatId { get; set; }
        public int SelectedDogId { get; set; }
    }
}