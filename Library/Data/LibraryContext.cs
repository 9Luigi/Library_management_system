using Microsoft.EntityFrameworkCore;
using Library.Data.Configurations;
using Library.Models;

namespace Library.Data
{
    internal class LibraryContext : DbContext
    {
        internal DbSet<Member> Members { get; set; } = null!;
        internal DbSet<Book> Books { get; set; } = null!;
        internal DbSet<Author> Authors { get; set; } = null!;

        // Конструктор по умолчанию (для дизайнера и миграций)
        public LibraryContext()
        {
        }

        // Конструктор с опциями (для фабрики)
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = true;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbOptions)
        {
            // Если options переданы в конструктор — ничего не делаем
            if (dbOptions.IsConfigured) return;

            DbProviderConfig.Apply(dbOptions);
            dbOptions.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
            dbOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
        }
    }
}