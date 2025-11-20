using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;

namespace yotm.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course?> GetByCodeAsync(string code);
        Task<IEnumerable<Course>> GetActiveCoursesAsync();
        Task<Course?> GetCourseWithApplicationsAsync(int courseId);
        Task<int> GetApprovedApplicationCountAsync(int courseId);
    }
}
