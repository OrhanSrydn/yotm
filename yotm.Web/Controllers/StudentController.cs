using Microsoft.AspNetCore.Mvc;
using yotm.Web.Models;
using yotm.Web.Models.ApiResponses;
using yotm.Web.Services;

namespace yotm.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IApiService _apiService;

        public StudentController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Authorization kontrolü
        private bool IsStudentLoggedIn()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var role = HttpContext.Session.GetString("UserRole");
            return !string.IsNullOrEmpty(token) && role == "Student";
        }

        // Ders Listesi
        [HttpGet]
        public async Task<IActionResult> Courses()
        {
            if (!IsStudentLoggedIn())
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("StudentLogin", "Auth");
            }

            try
            {
                var courses = await _apiService.GetAsync<List<CourseViewModel>>("courses");

                if (courses == null)
                {
                    ViewBag.ErrorMessage = "Dersler yüklenirken bir hata oluştu.";
                    return View(new List<CourseViewModel>());
                }

                var applications = await _apiService.GetAsync<List<ApplicationViewModel>>("courseapplications/me/applications");

                if (applications != null)
                {
                    var appliedCourseIds = applications.Select(a => a.CourseId).ToList();

                    foreach (var course in courses)
                    {
                        course.HasApplied = appliedCourseIds.Contains(course.Id);
                    }
                }

                return View(courses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(new List<CourseViewModel>());
            }
        }

        // Derse Başvur
        [HttpPost]
        public async Task<IActionResult> ApplyToCourse([FromBody]ApplyCourseRequest request)
        {
            if (!IsStudentLoggedIn())
            {
                return Json(new { success = false, message = "Lütfen giriş yapınız." });
            }

            try
            {
                var response = await _apiService.PostAsync<ApiMessageResponse>("courseapplications", new
                {
                    courseId = request.CourseId
                });

                if (response != null)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return Json(new { success = true, message = response.Message });
                }

                return Json(new { success = false, message = "Başvuru yapılırken bir hata oluştu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Başvurularım
        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            if (!IsStudentLoggedIn())
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("StudentLogin", "Auth");
            }

            try
            {
                var applications = await _apiService.GetAsync<List<ApplicationViewModel>>("courseapplications/me/applications");

                if (applications == null)
                {
                    ViewBag.ErrorMessage = "Başvurular yüklenirken bir hata oluştu.";
                    return View(new List<ApplicationViewModel>());
                }

                return View(applications);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(new List<ApplicationViewModel>());
            }
        }

        // Profil Görüntüle
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!IsStudentLoggedIn())
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("StudentLogin", "Auth");
            }

            try
            {
                var profile = await _apiService.GetAsync<StudentProfileViewModel>("students/me");

                if (profile == null)
                {
                    ViewBag.ErrorMessage = "Profil bilgileri yüklenirken bir hata oluştu.";
                    return View(new StudentProfileViewModel());
                }

                return View(profile);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(new StudentProfileViewModel());
            }
        }

        // Profil Güncelle
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm] StudentProfileViewModel model)
        {
            if (!IsStudentLoggedIn())
            {
                return Json(new { success = false, message = "Lütfen giriş yapınız." });
            }

            try
            {
                var response = await _apiService.PutAsync<ApiMessageResponse>("students/me", new
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    StudentNumber = model.StudentNumber,
                    Department = model.Department
                });

                if (response != null)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Profil bilgileriniz güncellendi!";
                    return Json(new { success = true, message = response.Message });
                }

                return Json(new { success = false, message = "Güncelleme yapılırken bir hata oluştu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
