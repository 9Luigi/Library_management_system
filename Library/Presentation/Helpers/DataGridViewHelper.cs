using Library.Presentation.Theme;
using System.Windows.Forms;

namespace Library.Presentation.Controllers
{
    internal static class DataGridViewHelper
    {
        /// <summary>
        /// Applies the current theme and column layout to a DataGridView. Call after binding data.
        /// </summary>
        /// <param name="view">The DataGridView to customize.</param>
        internal static void CustomizeDataGridView(DataGridView view)
        {
            // Colors, borders, header style
            AppTheme.StyleGrid(view);

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
        }

        /// <summary>
        /// Tries to retrieve the text of a cell in the selected row of the DataGridView.
        /// </summary>
        /// <param name="view">The DataGridView instance.</param>
        /// <param name="columnIndexToCheck">The index of the column to check (default is 0).</param>
        /// <returns>A tuple indicating whether a non-empty value was found and the value itself.</returns>
        internal static (bool, string) TryGetValueFromRow(DataGridView view, int columnIndexToCheck = 0)
        {
            // Check if any cell is selected
            if (view.CurrentCell == null)
                return (false, "");

            // Get the row index of the selected cell and ensure it is valid
            int rowIndex = view.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= view.Rows.Count)
                return (false, "");

            // Ensure the specified column index is valid
            if (columnIndexToCheck < 0 || columnIndexToCheck >= view.ColumnCount)
                return (false, "");

            string? value = view.Rows[rowIndex].Cells[columnIndexToCheck]?.Value?.ToString();
            return string.IsNullOrEmpty(value) ? (false, "") : (true, value);
        }
    }
}
