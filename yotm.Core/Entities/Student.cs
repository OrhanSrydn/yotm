using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yotm.Core.Entities
{
    public class Student : BaseEntity
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? StudentNumber { get; set; }
        public string? Department {  get; set; }

        public ICollection<CourseApplication> Applications { get; set; } = new List<CourseApplication>();

    }
}
