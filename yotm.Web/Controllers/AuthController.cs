using Microsoft.AspNetCore.Mvc;
using yotm.Web.Models;
using yotm.Web.Models.ApiResponses;
using yotm.Web.Services;

namespace yotm.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Öğrenci Giriş Sayfası
        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View();
        }

        // OTP İsteği
        [HttpPost]
        public async Task<IActionResult> StudentLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _apiService.PostAsync<OtpResponseViewModel>("auth/request-otp", new
                {
                    phoneNumber = model.PhoneNumber
                });

                if (response != null)
                {
                    TempData["PhoneNumber"] = model.PhoneNumber;
                    TempData["SuccessMessage"] = response.Message ?? "Doğrulama kodu telefonunuza gönderildi.";

                    // Development için kodu göster
                    if (!string.IsNullOrEmpty(response.Code))
                    {
                        TempData["OtpCode"] = response.Code;
                    }

                    return RedirectToAction(nameof(VerifyOtp));
                }

                ModelState.AddModelError("", "Kod gönderilirken bir hata oluştu.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        // OTP Doğrulama Sayfası
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            var phoneNumber = TempData["PhoneNumber"]?.ToString();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction(nameof(StudentLogin));
            }

            TempData.Keep("PhoneNumber");
            TempData.Keep("OtpCode");

            return View(new VerifyOtpViewModel { PhoneNumber = phoneNumber });
        }

        // OTP Doğrulama
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep("PhoneNumber");
                return View(model);
            }

            try
            {
                var response = await _apiService.PostAsync<AuthResponseViewModel>("auth/verify-otp", new
                {
                    phoneNumber = model.PhoneNumber,
                    code = model.Code
                });

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    HttpContext.Session.SetString("AuthToken", response.Token);
                    HttpContext.Session.SetString("UserRole", "Student");
                    HttpContext.Session.SetString("PhoneNumber", model.PhoneNumber);

                    _apiService.SetAuthToken(response.Token);

                    TempData["SuccessMessage"] = "Giriş başarılı!";
                    return RedirectToAction("Courses", "Student");
                }

                ModelState.AddModelError("", "Geçersiz veya süresi dolmuş kod.");
                TempData.Keep("PhoneNumber");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                TempData.Keep("PhoneNumber");
                return View(model);
            }
        }

        // Admin Giriş Sayfası
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Admin Giriş
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _apiService.PostAsync<AuthResponseViewModel>("auth/admin-login", new
                {
                    username = model.Username,
                    password = model.Password
                });

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    HttpContext.Session.SetString("AuthToken", response.Token);
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("Username", model.Username);

                    _apiService.SetAuthToken(response.Token);

                    TempData["SuccessMessage"] = "Giriş başarılı!";
                    return RedirectToAction("Dashboard", "Admin");
                }

                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        // Çıkış
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _apiService.ClearAuthToken();
            TempData["SuccessMessage"] = "Çıkış yapıldı.";
            return RedirectToAction("Index", "Home");
        }
    }
}
