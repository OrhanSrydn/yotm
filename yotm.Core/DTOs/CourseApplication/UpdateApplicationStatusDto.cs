using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Enums;

namespace yotm.Core.DTOs.CourseApplication
{
    public class UpdateApplicationStatusDto
    {
        public ApplicationStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
