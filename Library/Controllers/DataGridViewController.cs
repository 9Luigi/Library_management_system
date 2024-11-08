

namespace Library.Controllers
{
	internal static class DataGridViewController
	{
		internal static void CustomizeDataGridView(DataGridView view)
		{
			//styles
			view.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 57, 85);
			view.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			view.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			view.DefaultCellStyle.BackColor = Color.White;
			view.DefaultCellStyle.ForeColor = Color.Black;
			view.DefaultCellStyle.Font = new Font("Segoe UI", 9);
			view.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

			// cell hover
			view.DefaultCellStyle.SelectionBackColor = Color.FromArgb(81, 160, 219);
			view.DefaultCellStyle.SelectionForeColor = Color.White;

			// remove headers
			view.RowHeadersVisible = false;

			// full width
			view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			// remove vertical scrollbar
			view.ScrollBars = ScrollBars.Both;

			// borders
			view.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			view.BorderStyle = BorderStyle.None;
			view.GridColor = Color.FromArgb(221, 221, 221);
		}
	}
}
