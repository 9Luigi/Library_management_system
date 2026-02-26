using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.EFConfigurations;
using Library.Domain.Models;
namespace Library
{
    internal class LibraryContextForEFcore : DbContext
	{
		internal DbSet<Member> Members { get; set; } = null!;
		internal DbSet<Book> Books { get; set; } = null!;
		internal DbSet<Author> Authors { get; set; } = null!;
		// Конструктор по умолчанию (для дизайнера)
		public LibraryContextForEFcore()
		{
			// Пустой конструктор для миграций и дизайнера
		}

		// Конструктор с опциями (для фабрики)
		public LibraryContextForEFcore(DbContextOptions<LibraryContextForEFcore> options)
			: base(options)
		{
			// Отключаем ленивую загрузку для производительности //TODO : проверить нужно ли это
			ChangeTracker.LazyLoadingEnabled = false;

			// Настройка поведения при сохранении //TODO : проверить нужно ли это
			ChangeTracker.AutoDetectChangesEnabled = true;

			// Отключаем ненужные фичи для производительности //TODO : проверить нужно ли это
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		// Метод получения строки подключения ТОЛЬКО если не переданы options
		private string GetDefaultConnectionString()
		{
			return Properties.Settings.Default.LibraryConnectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder dbOptions)
		{
			// Если options уже настроены (переданы в конструктор), ничего не делаем
			if (dbOptions.IsConfigured) return;
			// Используем дефолтную строку подключения
			dbOptions.UseSqlServer(GetDefaultConnectionString());
			dbOptions.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
			// Настройки по умолчанию
			dbOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new EntityConfigurationForAuthor());
			modelBuilder.ApplyConfiguration(new EntityConfigurationForMember());
			modelBuilder.ApplyConfiguration(new EntityConfigurationForBook());
		}
	}
}
