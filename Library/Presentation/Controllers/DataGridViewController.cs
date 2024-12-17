using System.Drawing;
using System.Windows.Forms;

namespace Library.Presentation.Controllers
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
    }
}
