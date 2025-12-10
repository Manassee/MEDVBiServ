using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDVBiServ.Application.Interfaces
{
    public interface IBibleService
    {
        // 1. Alle Bücher (z.B. BookInfosDto: Nummer + Name + Kapitelanzahl)
        Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation);

        // 2. Alle Kapitel eines Buches (einfach nur Nummern)
        Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber);

        // 3. Alle Verse eines Kapitels
        Task<IReadOnlyList<BookInfos>> GetVersesFromChapterAsync(
            Translation translation, int bookNumber, int chapter);

        // 4. Einzelner Vers
        Task<BookInfos?> GetSingleVerseAsync(
            Translation translation, int bookNumber, int chapter, int verseNumber);

        // 5. Vers-Bereich (Range)
        Task<IReadOnlyList<BookInfos>> GetVerseRangeAsync(
            Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse);

        // Optional: per Id (z.B. für interne APIs / Paging)
        Task<BookInfos?> GetVerseByIdAsync(Translation translation, int id);
    }
}
