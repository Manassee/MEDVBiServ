using MEDVBiServ.Application.Enums;
using MEDVBiServ.Infrastructure.Repository.interfaces;

namespace MEDVBiServ.Infrastructure.Repository
{
    public class BibleRepositoryRouter : IBibleRepositoryRouter
    {
        private readonly IDeBibleRepository _de;
        private readonly IFrBibleRepository _fr;

        public BibleRepositoryRouter(IDeBibleRepository de, IFrBibleRepository fr)
        {
            _de = de;
            _fr = fr;
        }

        public IBibleRepository For(Translation t) => t switch
        {
            Translation.De => _de,
            Translation.Fr => _fr,
            _ => _de
        };
    }
}
