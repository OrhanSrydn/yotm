using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;

namespace yotm.Core.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Student?> GetStudentWithApplicationsAsync(int studentId);
    }
}
