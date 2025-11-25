namespace MEDVBiServ.Application.Dtos
{
    public class VerseDto
    {
        // Entspricht der Struktur der Tabelle "Verses" in der Datenbank
        public int Book { get; init; }
        public int Chapter { get; init; }
        public int Verse { get; init; }
        public string Text { get; init; } = "";
        public string? BookName { get; init; }
    }
}
