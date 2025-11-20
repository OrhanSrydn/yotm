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
    public class CourseApplicationRepository : Repository<CourseApplication>, ICourseApplicationRepository
    {
        public CourseApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CourseApplication>> GetApplicationsByCourseIdAsync(int courseId)
        {
            return await _dbSet.Include(a => a.Student).Where(a => a.CourseId == courseId).OrderBy(a => a.ApplicationDate).ToListAsync();
        }

        public async Task<IEnumerable<CourseApplication>> GetApplicationsByStudentIdAsync(int studentId)
        {
            return await _dbSet.Include(a => a.Course).Where(a => a.StudentId == studentId).OrderByDescending(a => a.ApplicationDate).ToListAsync();
        }

        public async Task<int> GetApprovedCountByCourseIdAsync(int courseId)
        {
            return await _dbSet.CountAsync(a => a.CourseId == courseId && a.Status == Core.Enums.ApplicationStatus.Approved);
        }

        public async Task<bool> HasStudentAppliedToCourseAsync(int studentId, int courseId)
        {
            return await _dbSet.AnyAsync(a => a.StudentId == studentId && a.CourseId == courseId);
        }
    }
}
