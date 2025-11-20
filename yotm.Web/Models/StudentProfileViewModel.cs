namespace yotm.Web.Models
{
    public class StudentProfileViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? StudentNumber { get; set; }
        public string? Department { get; set; }
    }
}
