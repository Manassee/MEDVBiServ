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
        // Bücher-Liste (für Auswahl / Übersicht)
        Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation);

        // Alle Kapitel eines Buches
        Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber);

        // Alle Verse eines Kapitels
        Task<IReadOnlyList<VerseDto>> GetVersesFromChapterAsync(
            Translation translation, int bookNumber, int chapter);

        // Einzelner Vers
        Task<VerseDto?> GetSingleVerseAsync(
            Translation translation, int bookNumber, int chapter, int verseNumber);

        // Vers-Bereich (z.B. Vers 3–10)
        Task<IReadOnlyList<VerseDto>> GetVerseRangeAsync(
            Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse);

        // Vers per Id
        Task<VerseDto?> GetVerseByIdAsync(Translation translation, int id);
    }
}
