namespace MEDVBiServ.App.Infrastructure.DbContext
{
    using MEDVBiServ.App.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// 
    /// </summary>
    public class DEDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DEDbContext"/> class using the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the <see cref="DEDbContext"/>. These options configure the database connection and
        /// other settings.</param>
        public DEDbContext(DbContextOptions<DEDbContext> options)
            : base(options) { } // Ensure the base constructor is called with options

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Meta> Meta => Set<Meta>();

        /// <summary>
        /// Ein Satz von Versen werden hier aufbewahrt.
        /// </summary>
        public DbSet<Bible> Verses => Set<Bible>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meta>(entity =>
            {
                // Tabllennamen festlegen
                entity.ToTable("meta");

                // Primärschlüssel festlegen
                entity.HasKey(m => m.Field);

                // Field als Pflichtfeld markieren
                entity.Property(m => m.Field).IsRequired();

                // Value kann null sein.
                entity.Property(m => m.Value);

            });

            modelBuilder.Entity<Bible>(entity =>
            {
                // Tabellenamen festlegen
                entity.ToTable("verses");

                // Primärschlüssel
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).ValueGeneratedOnAdd(); // Auto-Inkrement für ID
                entity.Property(v => v.Book).IsRequired(); // Buchnummer (erforderlich)
                entity.Property(v => v.Chapter).IsRequired(); // Kapitelnummer (erforderlich)
                entity.Property(v => v.Verse).IsRequired(); // Versnummer (erforderlich)
                entity.Property(v => v.Text).IsRequired(); // Text des Verses (erforderlich)

                // Sinnvolle Indizes für typische Abfragen
                entity.HasIndex(v => new { v.Book, v.Chapter, v.Verse });
                entity.HasIndex(v => v.Book);

            });

        }
    }
}
