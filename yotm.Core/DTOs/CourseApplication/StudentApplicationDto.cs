using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Enums;

namespace yotm.Core.DTOs.CourseApplication
{
    public class StudentApplicationDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public ApplicationStatus Status { get; set; }
        public string StatusText => Status switch
        {
            ApplicationStatus.Pending => "Beklemede",
            ApplicationStatus.Approved => "Onaylandı",
            ApplicationStatus.Rejected => "Reddedildi",
            _ => "Bilinmiyor"
        };
        public DateTime ApplicationDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? Notes { get; set; }
    }
}
