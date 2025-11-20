using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Enums;

namespace yotm.Core.Interfaces.Services
{
    public interface ICourseApplicationService
    {
        Task<CourseApplication> ApplyToCourseAsync(int studentId, int courseId);
        Task<IEnumerable<CourseApplication>> GetStudentApplicationsAsync(int studentId);
        Task<IEnumerable<CourseApplication>> GetCourseApplicationsAsync(int courseId);
        Task<CourseApplication> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, string processedBy, string? notes = null);
        Task<bool> HasStudentAppliedAsync(int studentId, int courseId);
    }
}
