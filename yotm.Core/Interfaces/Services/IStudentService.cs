using yotm.Core.DTOs.Student;
using yotm.Core.Entities;

namespace yotm.Core.Interfaces.Services
{
    public interface IStudentService
    {
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<Student?> GetStudentByPhoneNumberAsync(string phoneNumber);
        Task<Student> UpdateStudentProfileAsync(int studentId, UpdateStudentProfileDto updateDto);
    }
}
