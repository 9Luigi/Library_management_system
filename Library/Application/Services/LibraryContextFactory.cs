using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
	public static class DbContextFactory
	{
		private static string _connectionString = string.Empty;
		private static string GetConnectionString()
		{
			 _connectionString = Properties.Settings.Default.LibraryConnectionString;

			if (string.IsNullOrEmpty(_connectionString))
			{
				throw new InvalidOperationException(
					"Connection string 'LibraryConnectionString' not found in application settings.");
			}

			return _connectionString;
		}
		public static void Initialize(string connectionString)
		{
			 _connectionString = GetConnectionString() ??
				throw new ArgumentNullException(nameof(connectionString));
		}

		internal static LibraryContextForEFcore CreateForRead()
		{
			var options = new DbContextOptionsBuilder<LibraryContextForEFcore>()
				.UseSqlServer(_connectionString)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // Для чтения
				.Options;

			return new LibraryContextForEFcore(options);
		}

		internal static LibraryContextForEFcore CreateForUpdate()
		{
			var options = new DbContextOptionsBuilder<LibraryContextForEFcore>()
				.UseSqlServer(_connectionString)
				// Tracking включен по умолчанию для обновлений
				.Options;

			return new LibraryContextForEFcore(options);
		}

		internal static LibraryContextForEFcore CreateScoped()
		{
			var context = CreateForUpdate();
			// Дополнительные настройки если нужно
			return context;
		}
	}
}