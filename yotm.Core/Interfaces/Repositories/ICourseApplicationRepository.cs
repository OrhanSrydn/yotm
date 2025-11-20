using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;

namespace yotm.Core.Interfaces.Repositories
{
    public interface ICourseApplicationRepository : IRepository<CourseApplication>
    {
        Task<IEnumerable<CourseApplication>> GetApplicationsByStudentIdAsync(int studentId);
        Task<IEnumerable<CourseApplication>> GetApplicationsByCourseIdAsync(int courseId);
        Task<bool> HasStudentAppliedToCourseAsync(int studentId, int courseId);
        Task<int> GetApprovedCountByCourseIdAsync(int courseId);
    }
}
