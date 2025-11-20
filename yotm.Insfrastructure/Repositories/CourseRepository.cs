using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Interfaces.Repositories;
using yotm.Insfrastructure.Data;

namespace yotm.Insfrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).OrderBy(c => c.Department).ThenBy(c => c.Name).ToListAsync();
        }

        public async Task<int> GetApprovedApplicationCountAsync(int courseId)
        {
            return await _context.CoursesApplication.CountAsync(a => a.CourseId == courseId && a.Status == Core.Enums.ApplicationStatus.Approved);
        }

        public async Task<Course?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Course?> GetCourseWithApplicationsAsync(int courseId)
        {
            return await _dbSet.Include(c=> c.Applications).ThenInclude(a => a.Student).FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }
}
