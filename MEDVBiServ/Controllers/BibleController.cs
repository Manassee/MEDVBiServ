using Microsoft.AspNetCore.Mvc;


namespace MEDVBiServ.Controllers
{
   /* public class BibleController : ControllerBase
    {
        private readonly IBibleRepositoryRouter _router;
        private readonly ILanguageProvider _lang;

        public BibleController(IBibleRepositoryRouter router, ILanguageProvider lang)
        {
            _router = router;
            _lang = lang;
        }

        [HttpGet("book/{book:int}/chapter/{chapter:int}")]
        public async Task<IActionResult> GetChapter(int book, int chapter, CancellationToken ct)
        {
            
            var repo = _router.For(_lang.Resolve(HttpContext));
            var verses = await repo.GetChapterAsync(book, chapter, ct);

            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }
            return Ok(verses);
        }

        [HttpGet("ref/{reference}")]
        public async Task<IActionResult> GetByReference(string reference, CancellationToken ct)
        {
            var repo = _router.For(_lang.Resolve(HttpContext));
            var result = await repo.GetByReferenceAsync(reference, ct);
            return Ok(result);
        }
    }*/
}
