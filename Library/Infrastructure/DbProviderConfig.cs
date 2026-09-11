using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public static class DbProviderConfig
    {
        public static string ConnectionString =>
            $"Data Source={Path.Combine(AppContext.BaseDirectory, "library.db")}";

        public static void Apply(DbContextOptionsBuilder options)
        {
            options.UseSqlite(ConnectionString);
        }
    }
}