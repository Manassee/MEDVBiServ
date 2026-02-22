using MEDVBiServ.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Infrastructure.DbFactories
{
    internal class FRDbContextFactory : IDesignTimeDbContextFactory<FRDbContext>
    {
        public FRDbContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
