namespace yotm.Web.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quota { get; set; }
        public int ApprovedCount { get; set; }
        public int AvailableQuota => Quota - ApprovedCount;
        public string? Department { get; set; }
        public string? Faculty { get; set; }
        public string? Instructor { get; set; }
        public bool IsQuotaFull => ApprovedCount >= Quota;
        public bool HasApplied { get; set; }
    }
}
