using MEDVBiServ.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.Infrastructure.Repository
{
    public sealed class FrBibleRepository
        : BibleRepository<Infrastructure.DbContext.FRDbContext>, IFrBibleRepository
    {
        public FrBibleRepository(
            IDbContextFactory<Infrastructure.DbContext.FRDbContext> factory) // <<< FR, nicht DE
            : base(factory) { }
    }
}