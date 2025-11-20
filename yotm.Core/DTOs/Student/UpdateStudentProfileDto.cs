namespace yotm.Core.DTOs.Student
{
    public class UpdateStudentProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? StudentNumber { get; set; }
        public string? Department { get; set; }
    }
}
