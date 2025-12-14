// MEDVBiServ.Application/Services/BibleService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Application.Mapper;
using MEDVBiServ.Infrastructure.Interfaces;

namespace MEDVBiServ.Application.Services
{
    public class BibleService : IBibleService
    {
        private readonly IBibleVerseRepository _repository;
        private readonly ILanguageProvider _languageProvider;

        public BibleService(IBibleVerseRepository repository, ILanguageProvider languageProvider)
        {
            _repository = repository;
            _languageProvider = languageProvider;
        }

        /// <summary>
        /// Liefert alle Bücher für die angegebene Übersetzung.
        /// Buchnamen kommen aus ElberfelderMap/LouisSegondMap über den LanguageProvider.
        /// </summary>
        public async Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation); // "de" / "fr"
            var bookNumbers = await _repository.GetAllBooksAsync(languageCode);

            var result = bookNumbers
                .Select(bookNumber => new BookInfos
                {
                    Id = bookNumber,
                    Name = _languageProvider.GetBookName(translation, bookNumber),
                    // Optional: Code/Testament/Kapitelanzahl ergänzen
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Liefert alle Kapitelnummern eines Buches.
        /// </summary>
        public async Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var chapters = await _repository.GetChaptersAsync(bookNumber, languageCode);
            return chapters;
        }

        /// <summary>
        /// Liefert alle Verse eines Kapitels als VerseDto-Liste.
        /// </summary>
        public async Task<IReadOnlyList<VerseDto>> GetVersesFromChapterAsync(
            Translation translation, int bookNumber, int chapter)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entities = await _repository.GetVersesFromChapterAsync(bookNumber, chapter, languageCode);

            var result = entities
                .Select(v =>
                {
                    var bookName = _languageProvider.GetBookName(translation, v.Book);
                    return BibleVerseMapper.ToDto(v, bookName);
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Liefert einen einzelnen Vers (Buch + Kapitel + Versnummer).
        /// </summary>
        public async Task<VerseDto?> GetSingleVerseAsync(
            Translation translation, int bookNumber, int chapter, int verseNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entity = await _repository.GetSingleVerseAsync(bookNumber, chapter, verseNumber, languageCode);

            if (entity is null)
                return null;

            var bookName = _languageProvider.GetBookName(translation, entity.Book);
            return BibleVerseMapper.ToDto(entity, bookName);
        }

        /// <summary>
        /// Liefert einen Versbereich (z. B. Verse 3–10) als VerseDto-Liste.
        /// </summary>
        public async Task<IReadOnlyList<VerseDto>> GetVerseRangeAsync(
            Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entities = await _repository.GetVerseRangeAsync(bookNumber, chapter, fromVerse, toVerse, languageCode);

            var result = entities
                .Select(v =>
                {
                    var bookName = _languageProvider.GetBookName(translation, v.Book);
                    return BibleVerseMapper.ToDto(v, bookName);
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Holt einen Vers direkt über seine Id (z. B. für Paging/Admin).
        /// </summary>
        public async Task<VerseDto?> GetVerseByIdAsync(Translation translation, int id)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entity = await _repository.GetVersById(id, languageCode);

            if (entity is null)
                return null;

            var bookName = _languageProvider.GetBookName(translation, entity.Book);
            return BibleVerseMapper.ToDto(entity, bookName);
        }
    }
}
