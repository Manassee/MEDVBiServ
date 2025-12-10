using MEDVBiServ.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiServ.Infrastructure.Repository.interfaces
{
    public interface IBibleRepositoryRouter
    {
        IBibleRepository For(Translation t);
    }
}
