namespace yotm.Web.Models
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? Notes { get; set; }
    }
}
