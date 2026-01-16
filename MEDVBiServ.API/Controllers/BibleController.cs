using Microsoft.AspNetCore.Mvc;

using MEDVBiServ.Application.Interfaces;                 // IBibleService
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

        public BibleController(IBibleService service)
        {
            _service = service;
        }

        [HttpGet("books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks([FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var appTranslation = MapToAppTranslation(translation);
            var books = await _service.GetBooksAsync(appTranslation);
            return Ok(books);
        }

        [HttpGet("books/{bookNumber:int}/chapters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChapters(
            [FromRoute] int bookNumber,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var appTranslation = MapToAppTranslation(translation);
            var chapters = await _service.GetChaptersAsync(appTranslation, bookNumber);
            return Ok(chapters);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVersesFromChapter(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var appTranslation = MapToAppTranslation(translation);
            var verses = await _service.GetVersesFromChapterAsync(appTranslation, bookNumber, chapter);
            return Ok(verses);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/{verseNumber:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleVerse(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromRoute] int verseNumber,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var appTranslation = MapToAppTranslation(translation);
            var verse = await _service.GetSingleVerseAsync(appTranslation, bookNumber, chapter, verseNumber);
            return verse is null ? NotFound() : Ok(verse);
        }

        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVerseRange(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] int fromVerse,
            [FromQuery] int toVerse,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            if (fromVerse < 1 || toVerse < 1 || toVerse < fromVerse)
                return BadRequest("Ungültiger Versbereich. Beispiel: fromVerse=3&toVerse=10");

            var appTranslation = MapToAppTranslation(translation);
            var verses = await _service.GetVerseRangeAsync(appTranslation, bookNumber, chapter, fromVerse, toVerse);
            return Ok(verses);
        }

        [HttpGet("verses/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVerseById(
            [FromRoute] int id,
            [FromQuery] LanguageCode translation = LanguageCode.De)
        {
            var appTranslation = MapToAppTranslation(translation);
            var verse = await _service.GetVerseByIdAsync(appTranslation, id);
            return verse is null ? NotFound() : Ok(verse);
        }

        [HttpGet("verses/paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultDto<BibleVerseDto>>> GetVersesPaged(
            [FromQuery] GetBibleVersesRequest request,
            CancellationToken ct = default)
        {
            if (request.Page < 1)
                return BadRequest("page muss >= 1 sein.");

            if (request.PageSize < 1)
                return BadRequest("pageSize muss >= 1 sein.");

            // In deinem Application-Service wird Contracts.LanguageCode schon gemappt -> OK
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
