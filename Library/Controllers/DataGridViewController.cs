

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

			// width & hieght control
			view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; //auto width
			view.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //auto hieght
			if (view.Columns.Count > 0)
			{
				view.Columns[view.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; //last column full fill
			}
			foreach (DataGridViewColumn column in view.Columns) //wrap text if shoud be
			{
				column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			}

			// remove vertical scrollbar
			view.ScrollBars = ScrollBars.Both;

			// borders
			view.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			view.BorderStyle = BorderStyle.None;
			view.GridColor = Color.FromArgb(221, 221, 221);
		}
	}
}
