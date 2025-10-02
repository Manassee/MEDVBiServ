namespace MEDVBiServ.Infrastructure.DbContext
{
    using MEDVBiServ.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    public class FRDbContext : DbContext
    {
        public FRDbContext(DbContextOptions<FRDbContext> options) : base(options) { }
        public DbSet<Meta> Meta => Set<Meta>();
        public DbSet<Bible> Verses => Set<Bible>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meta>(e =>
            {
                e.ToTable("meta");
                e.HasKey(m => m.Field);
                e.Property(m => m.Field).IsRequired();
                e.Property(m => m.Value);
            });

            modelBuilder.Entity<Bible>(e =>
            {
                e.ToTable("verses");
                e.HasKey(v => v.Id);
                e.Property(v => v.Id).ValueGeneratedOnAdd();
                e.Property(v => v.Book).IsRequired();
                e.Property(v => v.Chapter).IsRequired();
                e.Property(v => v.Verse).IsRequired();
                e.Property(v => v.Text).IsRequired();
                e.HasIndex(v => new { v.Book, v.Chapter, v.Verse });
                e.HasIndex(v => v.Book);
            });
        }
    }

}
