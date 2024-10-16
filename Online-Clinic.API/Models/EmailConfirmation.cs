namespace Online_Clinic.API.Models
{
    public class EmailConfirmation
    {
        public string Email { get; set; }
        public int Code { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
