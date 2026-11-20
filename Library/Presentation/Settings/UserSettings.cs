using System.Text.Json;

namespace Library.Presentation.Settings
{
	/// <summary>
	/// Client preferences stored in %LocalAppData%\Library\settings.json.
	/// </summary>
	internal sealed class UserSettings
	{
		/// <summary>
		/// %LocalAppData%\Library: survives Clean/Rebuild (bin is wiped) and doesn't need admin rights.
		/// </summary>
		private static readonly string DataFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Library");

		public string Theme { get; set; } = "light";

		/// <summary>Address of Library.Server (the "http" profile in its launchSettings.json).</summary>
		public string ServerUrl { get; set; } = "http://localhost:5063";

		private static readonly string FilePath = Path.Combine(DataFolder, "settings.json");
		private static readonly string LegacyThemeFile = Path.Combine(DataFolder, "theme.txt");
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

			return settings;
		}

		internal void Save()
		{
			try
			{
				Directory.CreateDirectory(DataFolder);
				File.WriteAllText(FilePath, JsonSerializer.Serialize(this, JsonOptions));
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
			{
				// The setting still applies, it just won't be remembered
			}
		}
	}
}
