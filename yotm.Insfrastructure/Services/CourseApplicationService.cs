using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Enums;
using yotm.Core.Interfaces.Repositories;
using yotm.Core.Interfaces.Services;

namespace yotm.Insfrastructure.Services
{
    public class CourseApplicationService : ICourseApplicationService
    {
        private readonly ICourseApplicationRepository _applicationRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;

        public CourseApplicationService(
            ICourseApplicationRepository applicationRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository)
        {
            _applicationRepository = applicationRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public async Task<CourseApplication> ApplyToCourseAsync(int studentId, int courseId)
        {
            // Öğrenci var mı?
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new Exception("Öğrenci bulunamadı");
            }

            // Ders var mı?
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new Exception("Ders bulunamadı");
            }

            // Ders aktif mi?
            if (!course.IsActive)
            {
                throw new Exception("Bu ders şu anda aktif değil");
            }

            // Daha önce başvuru yapılmış mı?
            var hasApplied = await _applicationRepository.HasStudentAppliedToCourseAsync(studentId, courseId);
            if (hasApplied)
            {
                throw new Exception("Bu derse zaten başvuru yaptınız");
            }

            // Kontenjan dolu mu?
            var approvedCount = await _applicationRepository.GetApprovedCountByCourseIdAsync(courseId);
            if (approvedCount >= course.Quota)
            {
                throw new Exception("Bu dersin kontenjanı dolmuştur");
            }

            // Başvuru oluştur
            var application = new CourseApplication
            {
                StudentId = studentId,
                CourseId = courseId,
                Status = ApplicationStatus.Pending,
                ApplicationDate = DateTime.Now
            };

            return await _applicationRepository.AddAsync(application);
        }

        public async Task<IEnumerable<CourseApplication>> GetStudentApplicationsAsync(int studentId)
        {
            return await _applicationRepository.GetApplicationsByStudentIdAsync(studentId);
        }

        public async Task<IEnumerable<CourseApplication>> GetCourseApplicationsAsync(int courseId)
        {
            return await _applicationRepository.GetApplicationsByCourseIdAsync(courseId);
        }

        public async Task<CourseApplication> UpdateApplicationStatusAsync(
            int applicationId,
            ApplicationStatus status,
            string processedBy,
            string? notes = null)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new Exception("Başvuru bulunamadı");
            }

            // Eğer onaylanıyorsa kontenjan kontrolü yap
            if (status == ApplicationStatus.Approved)
            {
                var course = await _courseRepository.GetByIdAsync(application.CourseId);
                if (course != null)
                {
                    var approvedCount = await _applicationRepository.GetApprovedCountByCourseIdAsync(course.Id);
                    if (approvedCount >= course.Quota)
                    {
                        throw new Exception("Kontenjan dolu, başvuru onaylanamaz");
                    }
                }
            }

            application.Status = status;
            application.ProcessedDate = DateTime.Now;
            application.ProcessedBy = processedBy;
            application.Notes = notes;

            await _applicationRepository.UpdateAsync(application);
            return application;
        }

        public async Task<bool> HasStudentAppliedAsync(int studentId, int courseId)
        {
            return await _applicationRepository.HasStudentAppliedToCourseAsync(studentId, courseId);
        }
    }
}
