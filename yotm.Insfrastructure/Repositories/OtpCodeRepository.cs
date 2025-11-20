using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;
using yotm.Core.Interfaces.Repositories;
using yotm.Insfrastructure.Data;

namespace yotm.Insfrastructure.Repositories
{
    public class OtpCodeRepository : Repository<OtpCode>, IOtpCodeRepository
    {
        public OtpCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<OtpCode?> GetValidOtpAsync(string phoneNumber, string code)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.PhoneNumber == phoneNumber && o.Code == code && !o.IsUsed && o.ExpiresAt > DateTime.Now);
        }
    }
}
