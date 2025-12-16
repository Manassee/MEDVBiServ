using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Contracts.Requests
{
    public sealed class GetBibleVersesRequest
    {
        public string Language { get; init; } = "de";
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 50;
    }
}
