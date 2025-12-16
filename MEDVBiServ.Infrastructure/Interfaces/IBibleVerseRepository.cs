using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEDVBiServ.Domain.Entities;

namespace MEDVBiServ.Infrastructure.Interfaces
{
    public interface IBibleVerseRepository
    {
        // 0. (optional, weil du es schon nutzt)
        Task<List<Bible>> GetAllVersAsync(string language);
        Task<Bible?> GetVersById(int id, string language);

        // 1. Alle Bücher (nur Nummern, Namen macht der Service per Mapper)
        Task<List<int>> GetAllBooksAsync(string language);

        // 2. Alle Kapitel für ein Buch
        Task<List<int>> GetChaptersAsync(int bookNumber, string language);

        // 3. Alle Verse eines Kapitels
        Task<List<Bible>> GetVersesFromChapterAsync(
            int bookNumber, int chapter, string language);

        // 4. Einzelner Vers (Buch + Kapitel + Vers)
        Task<Bible?> GetSingleVerseAsync(
            int bookNumber, int chapter, int verseNumber, string language);

        // 5. Vers-Bereich (z. B. 3–10)
        Task<List<Bible>> GetVerseRangeAsync(
            int bookNumber, int chapter, int fromVerse, int toVerse, string language);

        Task<int> CountAsync(string language, CancellationToken ct = default);

        Task<List<Bible>> GetPagedAsync(string language, int page, int pageSize, CancellationToken ct = default);

        Task<int> CountVersesInChapterAsync(int bookNumber, int chapter, string languageCode, CancellationToken ct = default);

        Task<IReadOnlyList<Bible>> GetVersesFromChapterPagedAsync(
            int bookNumber, int chapter, string languageCode, int page, int pageSize, CancellationToken ct = default);

        Task<int> CountAsync(string languageCode, int? bookNumber, int? chapter, string? search, CancellationToken ct = default);

        Task<IReadOnlyList<Bible>> GetPagedAsync(
            string languageCode,
            int? bookNumber,
            int? chapter,
            string? search,
            int page,
            int pageSize,
            string sortBy,
            bool desc,
            CancellationToken ct = default);

    }
}
