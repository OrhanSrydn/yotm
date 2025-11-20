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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Student?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        public async Task<Student?> GetStudentWithApplicationsAsync(int studentId)
        {
            return await _dbSet.Include(s => s.Applications).ThenInclude(a => a.Course).FirstOrDefaultAsync(s => s.Id == studentId);
        }
    }
}
