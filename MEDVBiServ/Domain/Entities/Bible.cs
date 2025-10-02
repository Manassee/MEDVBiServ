namespace MEDVBiServ.Domain.Entities
{
    /// <summary>
    /// Represents a single verse, typically used in the context of poetry, scripture, or other structured text.
    /// </summary>
    public sealed class Bible
    {
        // Tabelle: verses
        public long Id { get; set; }

        /// <summary>Gets or sets 1..66 (kanonische Reihenfolge; 1 = Genesis, 66 = Offenbarung)</summary>
        public int Book { get; set; }

        /// <summary>
        /// Kapitelnummer innerhalb des Buches.
        /// </summar
        public int Chapter { get; set; }

        /// <summary>
        /// Die Versnummer die innerhalb des Kapitels sind.
        /// </summary>
        public int Verse { get; set; }

        /// <summary>
        /// Bibeltext.
        /// </summary>
        public string Text { get; set; } = null!;
    }
}
