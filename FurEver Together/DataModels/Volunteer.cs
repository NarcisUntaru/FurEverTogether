namespace FurEver_Together.DataModels
{
    public partial class Volunteer : BaseModel
    {
        public string Message { get; set; }

        // Navigation properties
        public int UserId { get; set; }

        public User User { get; set; }
    }
}