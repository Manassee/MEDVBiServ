using MEDVBiServ.App.Application.Enums;
using Microsoft.EntityFrameworkCore.Query;

namespace MEDVBiServ.App.Infrastructure.Repository.interfaces
{
    public interface IBibleRepositoryRouter
    {
        IBibleRepository For(Translation t);
    }
}
