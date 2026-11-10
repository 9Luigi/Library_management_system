namespace Library.Presentation.Theme
{
	/// <summary>
	/// Set of colors used by the whole UI. One instance per theme.
	/// </summary>
	internal sealed record ThemePalette(
		Color Background,
		Color Surface,
		Color SurfaceAlt,
		Color Border,
		Color Text,
		Color TextMuted,
		Color Accent,
		Color AccentHover,
		Color AccentText,
		Color Selection,
		Color SelectionText,
		Color GridHeader,
		Color GridHeaderText,
		Color GridRowAlt,
		Color Sidebar,
		Color SidebarText,
		Color SidebarHover,
		Color SidebarActive)
	{
		internal static readonly ThemePalette Light = new(
			Background: Color.FromArgb(243, 244, 246),
			Surface: Color.White,
			SurfaceAlt: Color.FromArgb(249, 250, 251),
			Border: Color.FromArgb(229, 231, 235),
			Text: Color.FromArgb(17, 24, 39),
			TextMuted: Color.FromArgb(107, 114, 128),
			Accent: Color.FromArgb(37, 99, 235),
			AccentHover: Color.FromArgb(29, 78, 216),
			AccentText: Color.White,
			Selection: Color.FromArgb(219, 234, 254),
			SelectionText: Color.FromArgb(30, 58, 138),
			GridHeader: Color.FromArgb(249, 250, 251),
			GridHeaderText: Color.FromArgb(75, 85, 99),
			GridRowAlt: Color.FromArgb(249, 250, 251),
			Sidebar: Color.FromArgb(17, 24, 39),
			SidebarText: Color.FromArgb(209, 213, 219),
			SidebarHover: Color.FromArgb(31, 41, 55),
			SidebarActive: Color.FromArgb(37, 99, 235));

		internal static readonly ThemePalette Dark = new(
			Background: Color.FromArgb(17, 24, 39),
			Surface: Color.FromArgb(31, 41, 55),
			SurfaceAlt: Color.FromArgb(39, 49, 65),
			Border: Color.FromArgb(55, 65, 81),
			Text: Color.FromArgb(243, 244, 246),
			TextMuted: Color.FromArgb(156, 163, 175),
			Accent: Color.FromArgb(59, 130, 246),
			AccentHover: Color.FromArgb(37, 99, 235),
			AccentText: Color.White,
			Selection: Color.FromArgb(30, 58, 138),
			SelectionText: Color.White,
			GridHeader: Color.FromArgb(39, 49, 65),
			GridHeaderText: Color.FromArgb(209, 213, 219),
			GridRowAlt: Color.FromArgb(35, 45, 60),
			Sidebar: Color.FromArgb(11, 15, 25),
			SidebarText: Color.FromArgb(156, 163, 175),
			SidebarHover: Color.FromArgb(31, 41, 55),
			SidebarActive: Color.FromArgb(59, 130, 246));
	}
}
