namespace MEDVBiServ.Domain.Entities;

public sealed class NotificationMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Author { get; set; } = "System";
    public string Text { get; set; } = string.Empty;

    // nur noch als "Anzeige"
    public DateTimeOffset CreatedAt { get; set; }

    // ✅ SQLite-sicher zum Sortieren
    public long CreatedAtUnixMs { get; set; }
}