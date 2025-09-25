using MEDVBiServ.App.Application.Dtos;

namespace MEDVBiServ.App.Infrastructure.Repository.interfaces
{
    public interface IBibleRepository
    {
        // Definiere alle Methoden, die für den Zugriff auf Vers-Daten erforderlich sind
        Task<IReadOnlyList<BookInfos>> GetBooksAsync(CancellationToken ct = default);
        Task<BookInfos?> GetBookAsync(int book, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetChapterNumbersAsync(int book, CancellationToken ct = default);

        Task<IReadOnlyList<VerseDto>> GetChapterAsync(int book, int chapter, CancellationToken ct = default);
        Task<VerseDto?> GetVerseAsync(int book, int chapter, int verse, CancellationToken ct = default);
        Task<IReadOnlyList<VerseDto>> GetRangeAsync(int book, int chapter, int fromVerse, int toVerse, CancellationToken ct = default);

        Task<IReadOnlyList<VerseDto>> GetByReferenceAsync(string reference, CancellationToken ct = default);
        Task<IReadOnlyList<VerseDto>> SearchAsync(string term, int? book = null, int limit = 50, CancellationToken ct = default);


    }
}
