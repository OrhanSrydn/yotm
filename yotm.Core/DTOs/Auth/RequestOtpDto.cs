using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yotm.Core.DTOs.Auth
{
    public class RequestOtpDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
