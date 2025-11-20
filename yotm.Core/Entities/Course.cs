using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yotm.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quota { get; set; }
        public string? Department { get; set; }
        public string? Faculty { get; set; }
        public string? Instructor { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<CourseApplication> Applications { get; set; } = new List<CourseApplication>();
    }
}
