using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using yotm.Core.DTOs.CourseApplication;
using yotm.Core.Interfaces.Services;

namespace yotm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseApplicationsController : ControllerBase
    {
        private readonly ICourseApplicationService _applicationService;
        public CourseApplicationsController(ICourseApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Derse başvuru yap (Öğrenci)
        /// </summary>
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> ApplyToCourse([FromBody] CreateApplicationDto request)
        {
            try
            {
                // Token'dan öğrenci ID'sini al
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
                {
                    return Unauthorized(new { message = "Geçersiz kullanıcı" });
                }

                var application = await _applicationService.ApplyToCourseAsync(studentId, request.CourseId);

                return Ok(new
                {
                    message = "Başvurunuz başarıyla alındı",
                    applicationId = application.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Öğrencinin tüm başvurularını listele
        /// </summary>
        [Authorize(Roles = "Student")]
        [HttpGet("me/applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
                {
                    return Unauthorized(new { message = "Geçersiz kullanıcı" });
                }

                var applications = await _applicationService.GetStudentApplicationsAsync(studentId);

                var applicationDtos = applications.Select(a => new StudentApplicationDto
                {
                    Id = a.Id,
                    CourseId = a.CourseId,
                    CourseCode = a.Course.Code,
                    CourseName = a.Course.Name,
                    Department = a.Course.Department,
                    Status = a.Status,
                    ApplicationDate = a.ApplicationDate,
                    ProcessedDate = a.ProcessedDate,
                    Notes = a.Notes
                }).ToList();

                return Ok(applicationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// Başvuru durumunu güncelle (Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusDto request)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";

                var application = await _applicationService.UpdateApplicationStatusAsync(
                    id,
                    request.Status,
                    username,
                    request.Notes
                );

                return Ok(new
                {
                    message = "Başvuru durumu güncellendi",
                    applicationId = application.Id,
                    status = application.Status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
