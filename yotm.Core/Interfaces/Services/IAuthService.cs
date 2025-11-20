using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yotm.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RequestOtpAsync(string phoneNumber);
        Task<string?> VerifyOtpAsync(string phoneNumber, string code);
        Task<string?> AdminLoginAsync(string username, string password);
        string GenerateJwtToken(string userId, string role);
    }
}
