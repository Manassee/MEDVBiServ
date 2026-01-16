using MEDVBiServ.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Contracts.Requests
{
    public sealed class GetBibleVersesRequest
    {
        public LanguageCode Language { get; init; } = LanguageCode.De;

        public int? BookNumber { get; init; }
        public int? Chapter { get; init; }

        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 50;
        public VerseSortBy SortBy { get; init; } = VerseSortBy.Book;
        public bool Desc { get; init; } = false;
        public string? Search { get; init; }

    }
}
