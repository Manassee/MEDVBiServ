using MEDVBiServ.App.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.App.Infrastructure.Repository
{
    /// <summary>
    /// Repository for accessing Bible data specific to the DE context.
    /// </summary>
    public sealed class DeBibleRepository
         : BibleRepository<MEDVBiServ.App.Infrastructure.DbContext.DEDbContext>, IDeBibleRepository
    {
        public DeBibleRepository(
            IDbContextFactory<MEDVBiServ.App.Infrastructure.DbContext.DEDbContext> factory) // <<< DE, nicht FR
            : base(factory) { }
    }
}
