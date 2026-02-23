using Microsoft.AspNetCore.Mvc;
using MEDVBiServ.Application.Interfaces;
using MEDVBiServ.Contracts.Enums;
using MEDVBiServ.Contracts.Requests;
using MEDVBiServ.Contracts.Dtos;

namespace MEDVBiServ.API.Controllers
{
    [ApiController]
    [Route("api/bible")]
    public sealed class BibleController : ControllerBase
    {
        private readonly IBibleService _service;

        public BibleController(IBibleService service) => _service = service;

        [HttpGet("books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<BookInfos>>> GetBooks(
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var books = await _service.GetBooksAsync(MapToAppTranslation(translation));
            return Ok(books);
        }

        [HttpGet("books/{bookNumber:int}/chapters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<int>>> GetChapters(
            [FromRoute] int bookNumber,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var chapters = await _service.GetChaptersAsync(MapToAppTranslation(translation), bookNumber);
            return Ok(chapters);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<BibleVerseDto>>> GetVersesFromChapter(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var verses = await _service.GetVersesFromChapterAsync(MapToAppTranslation(translation), bookNumber, chapter);
            return Ok(verses);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/{verseNumber:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BibleVerseDto>> GetSingleVerse(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromRoute] int verseNumber,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var verse = await _service.GetSingleVerseAsync(MapToAppTranslation(translation), bookNumber, chapter, verseNumber);
            return verse is null ? NotFound() : Ok(verse);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<BibleVerseDto>>> GetVerseRange(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] int fromVerse,
            [FromQuery] int toVerse,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            if (fromVerse < 1 || toVerse < 1 || toVerse < fromVerse)
                return BadRequest("Ungültiger Versbereich. Beispiel: fromVerse=3&toVerse=10");

            var verses = await _service.GetVerseRangeAsync(MapToAppTranslation(translation), bookNumber, chapter, fromVerse, toVerse);
            return Ok(verses);
        }

        [HttpGet("verses/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BibleVerseDto>> GetVerseById(
            [FromRoute] int id,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var verse = await _service.GetVerseByIdAsync(MapToAppTranslation(translation), id);
            return verse is null ? NotFound() : Ok(verse);
        }

        [HttpGet("verses/paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultDto<BibleVerseDto>>> GetVersesPaged(
            [FromQuery] GetBibleVersesRequest request,
            CancellationToken ct = default)
        {
            if (request.Page < 1) return BadRequest("page muss >= 1 sein.");
            if (request.PageSize < 1) return BadRequest("pageSize muss >= 1 sein.");

            var result = await _service.GetVersesPagedAsync(request, ct);
            return Ok(result);
        }

        private static MEDVBiServ.Application.Enums.Translation MapToAppTranslation(LanguageCode tr)
            => tr switch
            {
                LanguageCode.De => MEDVBiServ.Application.Enums.Translation.De,
                LanguageCode.Fr => MEDVBiServ.Application.Enums.Translation.Fr,
                _ => MEDVBiServ.Application.Enums.Translation.De
            };
    }
}