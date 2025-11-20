using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Interfaces.Repositories;
using yotm.Core.Interfaces.Services;

namespace yotm.Insfrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllActiveCoursesAsync()
        {
            return await _courseRepository.GetActiveCoursesAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _courseRepository.GetByIdAsync(courseId);
        }

        public async Task<Course?> GetCourseWithApplicationsAsync(int courseId)
        {
            return await _courseRepository.GetCourseWithApplicationsAsync(courseId);
        }

        public async Task<bool> IsQuotaAvailableAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            var approvedCount = await _courseRepository.GetApprovedApplicationCountAsync(courseId);
            return approvedCount < course.Quota;
        }
    }
}
