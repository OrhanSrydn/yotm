using Microsoft.AspNetCore.Mvc;
using yotm.Core.DTOs.Auth;
using yotm.Core.Interfaces.Services;

namespace yotm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// OTP kodu talep et (SMS gönder)
        /// </summary>
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                return BadRequest(new { message = "Telefon numarası gereklidir" });
            }

            if (request.PhoneNumber.Length < 10)
            {
                return BadRequest(new { message = "Geçersiz telefon numarası" });
            }

            try
            {
                var code = await _authService.RequestOtpAsync(request.PhoneNumber);

                return Ok(new
                {
                    message = "Doğrulama kodu gönderildi",
                    code = code, // SADECE DEVELOPMENT İÇİN!
                    expiresIn = 300 // 5 dakika
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// OTP kodunu doğrula ve giriş yap
        /// </summary>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest(new { message = "Telefon numarası ve kod gereklidir" });
            }

            try
            {
                var token = await _authService.VerifyOtpAsync(request.PhoneNumber, request.Code);

                if (token == null)
                {
                    return Unauthorized(new { message = "Geçersiz veya süresi dolmuş kod" });
                }

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Role = "Student",
                    UserId = request.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }

        /// <summary>
        /// Admin girişi (kullanıcı adı ve şifre ile)
        /// </summary>
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Kullanıcı adı ve şifre gereklidir" });
            }

            try
            {
                var token = await _authService.AdminLoginAsync(request.Username, request.Password);

                if (token == null)
                {
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Role = "Admin",
                    UserId = request.Username
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", error = ex.Message });
            }
        }
    }
}
