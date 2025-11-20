using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Enums;

namespace yotm.Core.Entities
{
    public class CourseApplication : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public DateTime? ProcessedDate { get; set; }
        public string? ProcessedBy { get; set; }
        public string? Notes { get; set; }

        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
