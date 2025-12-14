using MEDVBiServ.Application.Enums;
using MEDVBiServ.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Liefert alle Bücher (bookNumber + Name aus Mapping).
        /// GET /api/bible/books?translation=Elberfelder
        /// GET /api/bible/books?translation=LouisSegond
        /// </summary>
        [HttpGet("books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks([FromQuery] Translation translation = Translation.De)
        {
            var books = await _service.GetBooksAsync(translation);
            return Ok(books);
        }

        /// <summary>
        /// Liefert alle Kapitelnummern eines Buches.
        /// GET /api/bible/books/1/chapters?translation=Elberfelder
        /// </summary>
        [HttpGet("books/{bookNumber:int}/chapters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChapters(
            [FromRoute] int bookNumber,
            [FromQuery] Translation translation = Translation.De)
        {
            var chapters = await _service.GetChaptersAsync(translation, bookNumber);
            return Ok(chapters);
        }

        /// <summary>
        /// Liefert alle Verse eines Kapitels.
        /// GET /api/bible/books/1/chapters/1/verses?translation=Elberfelder
        /// </summary>
        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVersesFromChapter(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] Translation translation = Translation.De)
        {
            var verses = await _service.GetVersesFromChapterAsync(translation, bookNumber, chapter);
            return Ok(verses);
        }

        /// <summary>
        /// Liefert einen einzelnen Vers (book + chapter + verseNumber).
        /// GET /api/bible/books/1/chapters/1/verses/1?translation=Elberfelder
        /// </summary>
        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/{verseNumber:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleVerse(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromRoute] int verseNumber,
            [FromQuery] Translation translation = Translation.De)
        {
            var verse = await _service.GetSingleVerseAsync(translation, bookNumber, chapter, verseNumber);
            return verse is null ? NotFound() : Ok(verse);
        }

        /// <summary>
        /// Liefert einen Versbereich (fromVerse..toVerse).
        /// GET /api/bible/books/1/chapters/1/verses/range?fromVerse=3&toVerse=10&translation=Elberfelder
        /// </summary>
        [HttpGet("books/{bookNumber:int}/chapters/{chapter:int}/verses/range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVerseRange(
            [FromRoute] int bookNumber,
            [FromRoute] int chapter,
            [FromQuery] int fromVerse,
            [FromQuery] int toVerse,
            [FromQuery] Translation translation = Translation.De)
        {
            if (fromVerse < 1 || toVerse < 1 || toVerse < fromVerse)
                return BadRequest("Ungültiger Versbereich. Beispiel: fromVerse=3&toVerse=10");

            var verses = await _service.GetVerseRangeAsync(translation, bookNumber, chapter, fromVerse, toVerse);
            return Ok(verses);
        }

        /// <summary>
        /// Holt einen Vers über seine DB-Id (z.B. Admin/Paging).
        /// GET /api/bible/verses/123?translation=Elberfelder
        /// </summary>
        [HttpGet("verses/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVerseById(
            [FromRoute] int id,
            [FromQuery] Translation translation = Translation.De)
        {
            var verse = await _service.GetVerseByIdAsync(translation, id);
            return verse is null ? NotFound() : Ok(verse);
        }
    }
}
