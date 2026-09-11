using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public static class DbContextFactory
    {
        internal static LibraryContext CreateForRead()
        {
            var builder = new DbContextOptionsBuilder<LibraryContext>();
            DbProviderConfig.Apply(builder);
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new LibraryContext(builder.Options);
        }

        internal static LibraryContext CreateForUpdate()
        {
            var builder = new DbContextOptionsBuilder<LibraryContext>();
            DbProviderConfig.Apply(builder);
            // Tracking включён по умолчанию для обновлений

            return new LibraryContext(builder.Options);
        }
    }
}