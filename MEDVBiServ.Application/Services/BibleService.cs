using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IReadOnlyList<BookInfos>> GetBooksAsync(Translation translation)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation); // "de" / "fr"

            var bookNumbers = await _repository.GetAllBooksAsync(languageCode);

            var result = bookNumbers
                .Select(bookNumber => new BookInfos
                {
                    Id = bookNumber,
                    Name = _languageProvider.GetBookName(translation, bookNumber),
                    // Optional: Kapitelanzahl hier berechnen, wenn du willst
                })
                .ToList();

            return result;
        }

        public async Task<IReadOnlyList<int>> GetChaptersAsync(Translation translation, int bookNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var chapters = await _repository.GetChaptersAsync(bookNumber, languageCode);
            return chapters;
        }

        public async Task<BookInfos?> GetSingleVerseAsync(Translation translation, int bookNumber, int chapter, int verseNumber)
        {
            var languageCode = _languageProvider.GetLanguageCode(translation);
            var entity = await _repository.GetSingleVerseAsync(bookNumber, chapter, verseNumber, languageCode);

            return entity is null
                ? null
                : .ToDto(entity, translation);
        }

        public Task<BookInfos?> GetVerseByIdAsync(Translation translation, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<BookInfos>> GetVerseRangeAsync(Translation translation, int bookNumber, int chapter, int fromVerse, int toVerse)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<BookInfos>> GetVersesFromChapterAsync(Translation translation, int bookNumber, int chapter)
        {
            throw new NotImplementedException();
        }
    }
}
