using MEDVBiServ.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.Infrastructure.Repository
{
    /// <summary>
    /// Repository for accessing Bible data specific to the DE context.
    /// </summary>
    public sealed class DeBibleRepository
         : BibleRepository<Infrastructure.DbContext.DEDbContext>, IDeBibleRepository
    {
        public DeBibleRepository(
            IDbContextFactory<Infrastructure.DbContext.DEDbContext> factory) // <<< DE, nicht FR
            : base(factory) { }
    }
}
