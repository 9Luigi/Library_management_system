namespace Library.Presentation.Theme
{
	/// <summary>
	/// Renders MenuStrip / ContextMenuStrip with the current theme colors.
	/// Installed globally via ToolStripManager.Renderer, so every menu picks it up.
	/// </summary>
	internal sealed class ThemedMenuRenderer : ToolStripProfessionalRenderer
	{
		public ThemedMenuRenderer() : base(new ThemedColorTable(AppTheme.Palette))
		{
			RoundedEdges = false;
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = e.Item.Enabled ? AppTheme.Palette.Text : AppTheme.Palette.TextMuted;
			base.OnRenderItemText(e);
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			e.ArrowColor = AppTheme.Palette.Text;
			base.OnRenderArrow(e);
		}
	}

	internal sealed class ThemedColorTable(ThemePalette p) : ProfessionalColorTable
	{
		public override Color ToolStripDropDownBackground => p.Surface;
		public override Color ToolStripBorder => p.Border;
		public override Color MenuStripGradientBegin => p.Surface;
		public override Color MenuStripGradientEnd => p.Surface;
		public override Color MenuBorder => p.Border;
		public override Color MenuItemBorder => p.Selection;
		public override Color MenuItemSelected => p.Selection;
		public override Color MenuItemSelectedGradientBegin => p.Selection;
		public override Color MenuItemSelectedGradientEnd => p.Selection;
		public override Color MenuItemPressedGradientBegin => p.SurfaceAlt;
		public override Color MenuItemPressedGradientMiddle => p.SurfaceAlt;
		public override Color MenuItemPressedGradientEnd => p.SurfaceAlt;
		public override Color ImageMarginGradientBegin => p.Surface;
		public override Color ImageMarginGradientMiddle => p.Surface;
		public override Color ImageMarginGradientEnd => p.Surface;
		public override Color SeparatorDark => p.Border;
		public override Color SeparatorLight => p.Border;
	}
}
