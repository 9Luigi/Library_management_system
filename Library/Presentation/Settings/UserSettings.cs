using Library.Data;
using System.Text.Json;

namespace Library.Presentation.Settings
{
	/// <summary>
	/// Client preferences stored in %LocalAppData%\Library\settings.json.
	/// </summary>
	internal sealed class UserSettings
	{
		internal static readonly int[] PageSizes = [10, 25, 50, 100];
		private const int DefaultPageSize = 25;

		public string Theme { get; set; } = "light";
		public int MembersPageSize { get; set; } = DefaultPageSize;

		private static readonly string FilePath = Path.Combine(DbProviderConfig.DataFolder, "settings.json");
		private static readonly string LegacyThemeFile = Path.Combine(DbProviderConfig.DataFolder, "theme.txt");
		private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

		private static UserSettings? _current;
		internal static UserSettings Current => _current ??= Load();

		private static UserSettings Load()
		{
			var settings = new UserSettings();
			try
			{
				if (File.Exists(FilePath))
				{
					settings = JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(FilePath)) ?? new UserSettings();
				}
				else if (File.Exists(LegacyThemeFile))
				{
					// One-time move from the old theme.txt
					settings.Theme = File.ReadAllText(LegacyThemeFile).Trim() == "dark" ? "dark" : "light";
					settings.Save();
					File.Delete(LegacyThemeFile);
				}
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
			{
				// Broken or unreadable file: start with defaults
			}

			if (!PageSizes.Contains(settings.MembersPageSize)) settings.MembersPageSize = DefaultPageSize;
			return settings;
		}

		internal void Save()
		{
			try
			{
				Directory.CreateDirectory(DbProviderConfig.DataFolder);
				File.WriteAllText(FilePath, JsonSerializer.Serialize(this, JsonOptions));
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
			{
				// The setting still applies, it just won't be remembered
			}
		}
	}
}
