using yotm.Core.DTOs.Student;
using yotm.Core.Entities;
using yotm.Core.Interfaces.Repositories;
using yotm.Core.Interfaces.Services;

namespace yotm.Insfrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await _studentRepository.GetByIdAsync(studentId);
        }

        public async Task<Student?> GetStudentByPhoneNumberAsync(string phoneNumber)
        {
            return await _studentRepository.GetByPhoneNumberAsync(phoneNumber);
        }

        public async Task<Student> UpdateStudentProfileAsync(int studentId, UpdateStudentProfileDto updateDto)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new Exception("Öğrenci bulunamadı");
            }

            // Update student properties
            student.FirstName = updateDto.FirstName;
            student.LastName = updateDto.LastName;
            student.Email = updateDto.Email;
            student.StudentNumber = updateDto.StudentNumber;
            student.Department = updateDto.Department;

            await _studentRepository.UpdateAsync(student);

            return student;
        }
    }
}
