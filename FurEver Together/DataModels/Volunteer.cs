namespace FurEver_Together.DataModels
{
    public partial class Volunteer
    {
        public int Id { get; set; }
        public string Message { get; set; }

        // Navigation properties
        public string UserId { get; set; }

        public User User { get; set; }
    }
}