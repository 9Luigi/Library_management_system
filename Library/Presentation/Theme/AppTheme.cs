using Library.Presentation.Settings;
using System.Runtime.InteropServices;

namespace Library.Presentation.Theme
{
	internal enum ButtonRole
	{
		Primary,
		Secondary
	}

	/// <summary>
	/// Applies the light/dark theme to forms at runtime.
	/// Designer files keep only layout; colors and styles come from here.
	/// </summary>
	internal static class AppTheme
	{
		/// <summary>Put into Control.Tag to make the theme skip a control and its children (it styles itself).</summary>
		internal const string SkipTag = "theme:skip";

		internal static readonly Font HeaderFont = new("Segoe UI Semibold", 9F);
		internal static readonly Font TitleFont = new("Segoe UI Semibold", 16F);
		internal static readonly Font NavFont = new("Segoe UI", 10.5F);

		internal static bool IsDark { get; private set; }
		internal static ThemePalette Palette => IsDark ? ThemePalette.Dark : ThemePalette.Light;

		/// <summary>Raised after the theme was switched and all open forms were re-styled.</summary>
		internal static event EventHandler? Changed;

		/// <summary>Reads the saved theme. Call once at startup before the first form is created.</summary>
		internal static void Load()
		{
			IsDark = UserSettings.Current.Theme == "dark";
			ToolStripManager.Renderer = new ThemedMenuRenderer();
		}

		internal static void Toggle()
		{
			IsDark = !IsDark;
			UserSettings.Current.Theme = IsDark ? "dark" : "light";
			UserSettings.Current.Save();

			ToolStripManager.Renderer = new ThemedMenuRenderer();
			foreach (var form in System.Windows.Forms.Application.OpenForms.Cast<Form>().ToList())
			{
				Apply(form);
				form.Invalidate(true);
			}
			Changed?.Invoke(null, EventArgs.Empty);
		}

		/// <summary>Styles the form and all of its controls with the current palette.</summary>
		internal static void Apply(Form form)
		{
			var p = Palette;
			form.BackColor = p.Background;
			form.ForeColor = p.Text;
			if (form.TopLevel) ApplyTitleBar(form);
			ApplyChildren(form.Controls, p);
		}

		private static void ApplyChildren(Control.ControlCollection controls, ThemePalette p)
		{
			foreach (Control c in controls)
			{
				if (Equals(c.Tag, SkipTag)) continue;

				// Embedded form (e.g. a page inside the main window)
				if (c is Form embedded)
				{
					Apply(embedded);
					continue;
				}

				ApplyControl(c, p);
				if (c.HasChildren && c is not DataGridView) ApplyChildren(c.Controls, p);
			}
		}

		private static void ApplyControl(Control c, ThemePalette p)
		{
			switch (c)
			{
				case Button b:
					StyleButton(b, p);
					break;
				case TextBoxBase t:
					t.BorderStyle = BorderStyle.FixedSingle;
					t.BackColor = p.Surface;
					t.ForeColor = p.Text;
					break;
				case DataGridView g:
					StyleGrid(g);
					break;
				case ComboBox cb:
					cb.FlatStyle = FlatStyle.Flat;
					cb.BackColor = p.Surface;
					cb.ForeColor = p.Text;
					break;
				case ToolStrip ts:
					ts.BackColor = p.Surface;
					ts.ForeColor = p.Text;
					ts.Invalidate();
					break;
				case PictureBox pb:
					pb.BackColor = p.SurfaceAlt;
					break;
				case Label l:
					l.ForeColor = p.Text;
					break;
				case Panel panel:
					panel.BackColor = p.Background;
					break;
			}
		}

		/// <summary>Flat button: accent-filled for primary actions, outlined for secondary ones (Tag = ButtonRole.Secondary).</summary>
		internal static void StyleButton(Button b, ThemePalette p)
		{
			bool secondary = b.Tag is ButtonRole.Secondary;

			b.FlatStyle = FlatStyle.Flat;
			b.UseVisualStyleBackColor = false;
			b.Cursor = Cursors.Hand;
			b.FlatAppearance.BorderSize = secondary ? 1 : 0;
			b.FlatAppearance.BorderColor = p.Border;
			b.BackColor = secondary ? p.Surface : p.Accent;
			b.ForeColor = secondary ? p.Text : p.AccentText;
			b.FlatAppearance.MouseOverBackColor = secondary ? p.SurfaceAlt : p.AccentHover;
			b.FlatAppearance.MouseDownBackColor = secondary ? p.Border : p.AccentHover;
		}

		/// <summary>Colors, borders and row sizes of a grid. Column layout is handled by DataGridViewHelper.</summary>
		internal static void StyleGrid(DataGridView g)
		{
			var p = Palette;

			g.EnableHeadersVisualStyles = false; // otherwise header colors are ignored
			g.BackgroundColor = p.Surface;
			g.GridColor = p.Border;
			g.BorderStyle = BorderStyle.None;
			g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			g.RowHeadersVisible = false;
			g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			g.ColumnHeadersHeight = 40;
			g.RowTemplate.Height = 36;

			var header = g.ColumnHeadersDefaultCellStyle;
			header.BackColor = p.GridHeader;
			header.ForeColor = p.GridHeaderText;
			header.SelectionBackColor = p.GridHeader;
			header.SelectionForeColor = p.GridHeaderText;
			header.Font = HeaderFont;
			header.Padding = new Padding(8, 0, 8, 0);

			var cell = g.DefaultCellStyle;
			cell.BackColor = p.Surface;
			cell.ForeColor = p.Text;
			cell.SelectionBackColor = p.Selection;
			cell.SelectionForeColor = p.SelectionText;
			cell.Padding = new Padding(8, 4, 8, 4);

			g.AlternatingRowsDefaultCellStyle.BackColor = p.GridRowAlt;
			g.AlternatingRowsDefaultCellStyle.ForeColor = p.Text;
		}

		#region Title bar

		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

		private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

		/// <summary>Dark/light window caption (Windows 10 20H1+ / Windows 11; ignored on older systems).</summary>
		private static void ApplyTitleBar(Form form)
		{
			if (!form.IsHandleCreated)
			{
				form.HandleCreated -= OnHandleCreated;
				form.HandleCreated += OnHandleCreated;
				return;
			}

			int value = IsDark ? 1 : 0;
			_ = DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
		}

		private static void OnHandleCreated(object? sender, EventArgs e)
		{
			if (sender is Form form) ApplyTitleBar(form);
		}

		#endregion
	}
}
