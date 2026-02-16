using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEDVBiServ.Infrastructure.DbContext;

namespace MEDVBiServ.Infrastructure.DbFactories
{
    internal class DEDbContextFactory : IDesignTimeDbContextFactory<DEDbContext>
    {
        public DEDbContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
