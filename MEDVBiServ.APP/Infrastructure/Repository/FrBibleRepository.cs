using MEDVBiServ.App.Infrastructure.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace MEDVBiServ.App.Infrastructure.Repository
{
    public sealed class FrBibleRepository
        : BibleRepository<MEDVBiServ.App.Infrastructure.DbContext.FRDbContext>, IFrBibleRepository
    {
        public FrBibleRepository(
            IDbContextFactory<MEDVBiServ.App.Infrastructure.DbContext.FRDbContext> factory) // <<< FR, nicht DE
            : base(factory) { }
    }
}