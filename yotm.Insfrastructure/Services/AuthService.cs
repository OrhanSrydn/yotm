using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Interfaces.Repositories;
using yotm.Core.Interfaces.Services;

namespace yotm.Insfrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpCodeRepository _otpRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAdminUserRepository _adminRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IOtpCodeRepository otpRepository,IStudentRepository studentRepository,IAdminUserRepository adminRepository,IConfiguration configuration)
        {
            _otpRepository = otpRepository;
            _studentRepository = studentRepository;
            _adminRepository = adminRepository;
            _configuration = configuration;
        }
        public async Task<string?> AdminLoginAsync(string username, string password)
        {
            var admin = await _adminRepository.GetByUsernameAsync(username);

            if (admin == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            var token = GenerateJwtToken(admin.Id.ToString(), "Admin");
            return token;
        }

        public string GenerateJwtToken(string userId, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YazOkuluSecretKey12345678901234567890";
            var issuer = jwtSettings["Issuer"] ?? "YazOkuluAPI";
            var audience = jwtSettings["Audience"] ?? "YazOkuluClient";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> RequestOtpAsync(string phoneNumber)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            var otpCode = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                ExpiresAt = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            await _otpRepository.AddAsync(otpCode);

            // Gerçek SMS servisi yerine konsola yazdırıyoruz (development için)
            Console.WriteLine($"SMS Gönderildi: {phoneNumber} - Kod: {code}");

            // Production'da buraya SMS servisi entegre edilir
            // await _smsService.SendSmsAsync(phoneNumber, $"Doğrulama kodunuz: {code}");

            return code;
        }

        public async Task<string?> VerifyOtpAsync(string phoneNumber, string code)
        {
            var otpCode = await _otpRepository.GetValidOtpAsync(phoneNumber,code);

            if(otpCode == null)
            {
                return null; //Geçersiz veya süresi dolmuşsa
            }

            otpCode.IsUsed = true;
            await _otpRepository.UpdateAsync(otpCode);

            var student = await _studentRepository.GetByPhoneNumberAsync(phoneNumber);

            if(student == null)
            {
                student = new Student
                {
                    PhoneNumber = phoneNumber,
                    FirstName = "Öğrenci",
                    LastName = phoneNumber.Substring(phoneNumber.Length - 6)
                };

                student = await _studentRepository.AddAsync(student);
            }

            var token = GenerateJwtToken(student.Id.ToString(), "Student");
            return token;
        }
    }
}
