using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;

namespace yotm.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllActiveCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<Course?> GetCourseWithApplicationsAsync(int courseId);
        Task<bool> IsQuotaAvailableAsync(int courseId);
    }
}
