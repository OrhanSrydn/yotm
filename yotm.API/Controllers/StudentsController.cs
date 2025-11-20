using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using yotm.Core.DTOs.Student;
using yotm.Core.Interfaces.Services;

namespace yotm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Giriş yapmış öğrencinin profil bilgilerini getir
        /// </summary>
        [Authorize(Roles = "Student")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
                {
                    return Unauthorized(new { message = "Geçersiz kullanıcı" });
                }

                var student = await _studentService.GetStudentByIdAsync(studentId);

                if (student == null)
                {
                    return NotFound(new { message = "Öğrenci bulunamadı" });
                }

                var profileDto = new StudentProfileDto
                {
                    Id = student.Id,
                    PhoneNumber = student.PhoneNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    StudentNumber = student.StudentNumber,
                    Department = student.Department
                };

                return Ok(profileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// Öğrenci profil bilgilerini güncelle
        /// </summary>
        [Authorize(Roles = "Student")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateStudentProfileDto updateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int studentId))
                {
                    return Unauthorized(new { message = "Geçersiz kullanıcı" });
                }

                var student = await _studentService.UpdateStudentProfileAsync(studentId, updateDto);

                return Ok(new
                {
                    message = "Profil bilgileriniz güncellendi",
                    student = new StudentProfileDto
                    {
                        Id = student.Id,
                        PhoneNumber = student.PhoneNumber,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        StudentNumber = student.StudentNumber,
                        Department = student.Department
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
