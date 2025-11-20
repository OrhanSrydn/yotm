using Microsoft.AspNetCore.Mvc;
using yotm.Web.Models;
using yotm.Web.Models.ApiResponses;
using yotm.Web.Services;

namespace yotm.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Authorization kontrolü
        private bool IsAdminLoggedIn()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var role = HttpContext.Session.GetString("UserRole");
            return !string.IsNullOrEmpty(token) && role == "Admin";
        }

        // Admin Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessage"] = "Lütfen admin olarak giriş yapınız.";
                return RedirectToAction("AdminLogin", "Auth");
            }

            try
            {
                var courses = await _apiService.GetAsync<List<CourseViewModel>>("courses");

                if (courses == null)
                {
                    ViewBag.ErrorMessage = "Dersler yüklenirken bir hata oluştu.";
                    return View(new List<CourseViewModel>());
                }

                return View(courses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(new List<CourseViewModel>());
            }
        }

        // Ders Başvurularını Görüntüle
        [HttpGet]
        public async Task<IActionResult> CourseApplications(int courseId)
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessage"] = "Lütfen admin olarak giriş yapınız.";
                return RedirectToAction("AdminLogin", "Auth");
            }

            try
            {
                var course = await _apiService.GetAsync<CourseViewModel>($"courses/{courseId}");

                if (course == null)
                {
                    TempData["ErrorMessage"] = "Ders bulunamadı.";
                    return RedirectToAction(nameof(Dashboard));
                }

                ViewBag.Course = course;

                var applications = await _apiService.GetAsync<List<AdminApplicationViewModel>>($"courses/{courseId}/applications");

                if (applications == null)
                {
                    ViewBag.ErrorMessage = "Başvurular yüklenirken bir hata oluştu.";
                    return View(new List<AdminApplicationViewModel>());
                }

                return View(applications);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(new List<AdminApplicationViewModel>());
            }
        }

        // Başvuru Durumunu Güncelle (Onayla/Reddet)
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusRequest request)
        {
            if (!IsAdminLoggedIn())
            {
                return Json(new { success = false, message = "Lütfen admin olarak giriş yapınız." });
            }

            try
            {
                int statusValue = request.Status switch
                {
                    "Approved" => 1,
                    "Rejected" => 2,
                    _ => 0
                };

                var response = await _apiService.PutAsync<ApiMessageResponse>($"courseapplications/{request.ApplicationId}/status", new
                {
                    Status = statusValue,
                    Notes = request.Notes
                });

                if (response != null)
                {
                    return Json(new
                    {
                        success = true,
                        message = response.Message ?? (request.Status == "Approved" ? "Başvuru onaylandı!" : "Başvuru reddedildi!")
                    });
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
