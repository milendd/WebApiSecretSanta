using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Data
{
    public class DbFactory : IDbFactory
    {
        SantaSystemDbContext dbContext;

        public SantaSystemDbContext GetContext()
        {
            return dbContext ?? (dbContext = new SantaSystemDbContext());
        }
    }
}
