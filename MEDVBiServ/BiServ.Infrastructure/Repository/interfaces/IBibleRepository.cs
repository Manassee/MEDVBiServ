using MEDVBiServ.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiServ.Infrastructure.Repository.interfaces
{
    public interface IBibleRepository
    {
        Task<IReadOnlyList<BookInfos>> GetAllBooksAsync(CancellationToken ct = default);
        Task<BookInfos?> GetBookAsync(int book, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetChapterNumbersAsync(int book, CancellationToken ct = default);


        Task<IReadOnlyList<VerseDto>> GetChapterAsync(int book, int chapter, CancellationToken ct = default);
        Task<VerseDto?> GetVerseAsync(int book, int chapter, int verse, CancellationToken ct = default);
        Task<IReadOnlyList<VerseDto>> GetRangeAsync(int book, int chapter, int fromVerse, int toVerse, CancellationToken ct = default);


        Task<IReadOnlyList<VerseDto>> GetByReferenceAsync(string reference, CancellationToken ct = default);
        Task<IReadOnlyList<VerseDto>> SearchAsync(string term,int? book = null, int limit = 50, CancellationToken ct = default);
    }
}
