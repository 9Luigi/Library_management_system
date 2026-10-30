using System.Drawing;
using System.Windows.Forms;

namespace Library.Presentation.Controllers
{
    internal static class DataGridViewHelper
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
        /// Tries to retrieve a value from the selected row of the DataGridView.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve (int or long).</typeparam>
        /// <param name="view">The DataGridView instance.</param>
        /// <param name="columnIndexToCheck">The index of the column to check (default is 0).</param>
        /// <returns>A tuple indicating whether the value was found and the value itself.</returns>
        internal static (bool, T) TryGetValueFromRow<T>(DataGridView view, int columnIndexToCheck = 0) where T : struct
        {
            // Check if any cell is selected
            if (view.CurrentCell == null)
                return (false, default);

            // Get the row index of the selected cell and ensure it is valid
            int rowIndex = view.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= view.Rows.Count)
                return (false, default);

            // Ensure the specified column index is valid
            if (columnIndexToCheck < 0 || columnIndexToCheck >= view.ColumnCount)
                return (false, default);

            // Check the value in the specified column of the selected row
            var cellValue = view.Rows[rowIndex].Cells[columnIndexToCheck]?.Value;
            if (cellValue != null && TryParseValue(cellValue.ToString()!, out T value))
            {
                return (true, value);
            }

            return (false, default);
        }

        private static bool TryParseValue<T>(string value, out T result) where T : struct
        {
            if (typeof(T) == typeof(long) && long.TryParse(value, out var longResult))
            {
                result = (T)(object)longResult;
                return true;
            }

            if (typeof(T) == typeof(int) && int.TryParse(value, out var intResult))
            {
                result = (T)(object)intResult;
                return true;
            }

            result = default;
            return false;
        }
    }
}
