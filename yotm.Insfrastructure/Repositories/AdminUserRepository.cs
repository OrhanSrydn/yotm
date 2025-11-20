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
    public class AdminUserRepository : Repository<AdminUser>, IAdminUserRepository
    {
        public AdminUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<AdminUser?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.UserName == username);
        }
    }
}
