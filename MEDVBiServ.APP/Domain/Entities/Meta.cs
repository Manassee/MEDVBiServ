namespace MEDVBiServ.App.Domain.Entities
{
    /// <summary>
    /// Repräsentiert einen Metadateneintrag, der zusätzliche Informationen zu verschiedenen Entitäten in der Anwendung speichert.
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Tabelle für Metadaten zu verschiedenen Entitäten.
        /// </summary>
        public string Field { get; set; } = null!;

        /// <summary>
        /// Der Wert des Metadateneintrags.
        /// </summary>
        public string? Value { get; set; }
    }
}
