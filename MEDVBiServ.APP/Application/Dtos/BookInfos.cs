using MEDVBiServ.App.Domain.Enums;

namespace MEDVBiServ.App.Application.Dtos
{
    /// <summary>
    /// Representiert Informationen über ein Buch der Bibel.
    /// </summary>
    public record BookInfos
    {
        public int Id { get; init; }            // 1..66 (entspricht Bible.Book)
        public string Code { get; init; } = ""; // z.B. "Gen", "Ps", "Joh", "1Joh"
        public string Name { get; init; } = ""; // z.B. "1. Mose", "Psalmen", "Johannes", "1. Johannes"
        public Testament Testament { get; init; }
        public int Chapters { get; init; }      // max. Kapitelnummer
    }
}
