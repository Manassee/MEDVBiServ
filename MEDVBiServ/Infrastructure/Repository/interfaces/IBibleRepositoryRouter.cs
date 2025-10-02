using MEDVBiServ.Application.Enums;

namespace MEDVBiServ.Infrastructure.Repository.interfaces
{
    public interface IBibleRepositoryRouter
    {
        IBibleRepository For(Translation t);
    }
}
