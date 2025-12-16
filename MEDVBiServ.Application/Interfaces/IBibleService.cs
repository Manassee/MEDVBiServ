using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Contracts.Dtos;

namespace MEDVBiServ.Application.Interfaces
{
    public interface IBibleService
    {
        Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation);

        Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber);

        Task<IReadOnlyList<VerseDto>> GetVersesFromChapterAsync(
            Translation translation, int bookNumber, int chapter);

        Task<VerseDto?> GetSingleVerseAsync(
            Translation translation, int bookNumber, int chapter, int verseNumber);

        Task<IReadOnlyList<VerseDto>> GetVerseRangeAsync(
            Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse);

        Task<VerseDto?> GetVerseByIdAsync(Translation translation, int id);

        
        Task<PagedResultDto<VerseDto>> GetVersesPagedAsync(
            Translation translation, int bookNumber, int chapter, int page, int pageSize,
            CancellationToken ct = default);
    }
}
