namespace yotm.Web.Models
{
    public class AdminApplicationViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? StudentNumber { get; set; }
        public string? Department { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? ProcessedBy { get; set; }
        public string? Notes { get; set; }
    }
}
