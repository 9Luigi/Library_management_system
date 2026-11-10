using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public static class DbProviderConfig
    {
        /// <summary>
        /// %LocalAppData%\Library: the database and the Photos folder live here,
        /// so they survive Clean/Rebuild (bin is wiped) and don't need admin rights.
        /// </summary>
        public static readonly string DataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Library");

        public static string ConnectionString =>
            $"Data Source={Path.Combine(DataFolder, "library.db")}";

        public static void Apply(DbContextOptionsBuilder options)
        {
            // SQLite creates the file, but not the folder
            Directory.CreateDirectory(DataFolder);
            options.UseSqlite(ConnectionString);
        }
    }
}
