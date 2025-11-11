using FurEver_Together.DataModels;
using System.Text.Json.Serialization;

namespace FurEver_Together.ViewModels
{
    public class ContactUsViewModel
    {
        [JsonRequired]
        public int Id { get; set; }
        [JsonRequired]
        public string Message { get; set; }
    }
}