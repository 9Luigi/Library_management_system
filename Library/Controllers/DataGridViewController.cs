using System.Drawing;
using System.Windows.Forms;

namespace Library.Controllers
{
	internal static class DataGridViewController
	{
		/// <summary>
		/// Applies custom styles to a DataGridView.
		/// </summary>
		/// <param name="view">The DataGridView to customize.</param>
		internal static void CustomizeDataGridView(DataGridView view)
		{
			// Styles
			view.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 57, 85);
			view.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			view.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			view.DefaultCellStyle.BackColor = Color.White;
			view.DefaultCellStyle.ForeColor = Color.Black;
			view.DefaultCellStyle.Font = new Font("Segoe UI", 9);
			view.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

			// Cell hover
			view.DefaultCellStyle.SelectionBackColor = Color.FromArgb(81, 160, 219);
			view.DefaultCellStyle.SelectionForeColor = Color.White;

			// Remove headers
			view.RowHeadersVisible = false;

			// Width & height control
			view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Auto width
			view.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Auto height
			if (view.Columns.Count > 0)
			{
				view.Columns[view.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Last column full fill
			}
			foreach (DataGridViewColumn column in view.Columns) // Wrap text if needed
			{
				column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			}

			// Scrollbars
			view.ScrollBars = ScrollBars.Both;

			// Borders
			view.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			view.BorderStyle = BorderStyle.None;
			view.GridColor = Color.FromArgb(221, 221, 221);
		}

		/// <summary>
		/// Checks the value in the specified column of the selected row in a DataGridView.
		/// </summary>
		/// <param name="view">The DataGridView to analyze.</param>
		/// <param name="columnIndexToCheck">The index of the column to check for the value.</param>
		/// <param name="IIN">An output parameter for the found IIN value.</param>
		/// <returns>A tuple where the first element indicates success, and the second contains the IIN value.</returns>
		internal static (bool, long) TryGetIINFromRow(DataGridView view, int columnIndexToCheck = 0)
		{
			// Check if any cell is selected
			if (view.CurrentCell == null)
				return (false, 0);

			// Get the row index of the selected cell and ensure it is valid
			int rowIndex = view.CurrentCell.RowIndex;
			if (rowIndex < 0 || rowIndex >= view.Rows.Count)
				return (false, 0);

			// Ensure the specified column index is valid
			if (columnIndexToCheck < 0 || columnIndexToCheck >= view.ColumnCount)
				return (false, 0);

			// Check the value in the specified column of the selected row
			var cellValue = view.Rows[rowIndex].Cells[columnIndexToCheck]?.Value;
			if (cellValue != null && long.TryParse(cellValue.ToString(), out long IIN))
			{
				return (true, IIN);
			}

			return (false, 0);
		}
	}
}
