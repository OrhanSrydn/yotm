using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yotm.Core.DTOs.Course;
using yotm.Core.DTOs.CourseApplication;
using yotm.Core.Interfaces.Repositories;
using yotm.Core.Interfaces.Services;

namespace yotm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ICourseRepository _courseRepository;
        public CoursesController(ICourseService courseService,ICourseRepository courseRepository)
        {
            _courseService = courseService;
            _courseRepository = courseRepository;
        }

        /// <summary>
        /// Tüm aktif dersleri listele
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllActiveCoursesAsync();

                var courseDtos = new List<CourseListDto>();

                foreach (var course in courses)
                {
                    var approvedCount = await _courseRepository.GetApprovedApplicationCountAsync(course.Id);

                    courseDtos.Add(new CourseListDto
                    {
                        Id = course.Id,
                        Code = course.Code,
                        Name = course.Name,
                        Quota = course.Quota,
                        ApprovedCount = approvedCount,
                        Department = course.Department,
                        Faculty = course.Faculty,
                        Instructor = course.Instructor
                    });
                }

                return Ok(courseDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// Belirli bir dersin detayını getir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);

                if (course == null)
                {
                    return NotFound(new { message = "Ders bulunamadı" });
                }

                var approvedCount = await _courseRepository.GetApprovedApplicationCountAsync(id);

                var courseDto = new CourseDetailDto
                {
                    Id = course.Id,
                    Code = course.Code,
                    Name = course.Name,
                    Quota = course.Quota,
                    ApprovedCount = approvedCount,
                    Department = course.Department,
                    Faculty = course.Faculty,
                    Instructor = course.Instructor
                };

                return Ok(courseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// Bir derse yapılan tüm başvuruları listele (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{courseId}/applications")]
        public async Task<IActionResult> GetCourseApplications(int courseId)
        {
            try
            {
                var course = await _courseService.GetCourseWithApplicationsAsync(courseId);

                if (course == null)
                {
                    return NotFound(new { message = "Ders bulunamadı" });
                }

                var applicationList = course.Applications.Select(a => new AdminApplicationDto
                {
                    Id = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student?.FirstName ?? "Bilinmiyor",
                    PhoneNumber = a.Student?.PhoneNumber ?? "",
                    StudentNumber = a.Student?.StudentNumber,
                    Department = a.Student?.Department,
                    Status = a.Status,
                    ApplicationDate = a.ApplicationDate,
                    ProcessedDate = a.ProcessedDate,
                    ProcessedBy = a.ProcessedBy,  // ← Admin'daki property adı ne?
                    Notes = a.Notes
                }).ToList();

                return Ok(applicationList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }
    }
}
