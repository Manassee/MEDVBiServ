using BiServ.Infrastructure.Repository.interfaces;
using MEDVBiServ.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiServ.Infrastructure.Repository
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
            Translation.De => throw new NotImplementedException(),
            Translation.Fr => throw new NotImplementedException(),
            
        };
    }
}
