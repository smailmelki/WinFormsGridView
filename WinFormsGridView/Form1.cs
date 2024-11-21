namespace WinFormsGridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // ÅäÔÇÁ ÇáÃÚãÏÉ
            dataGridView1.Columns.Add("ScreenName", "Screen Caption");
            dataGridView1.Columns.Add(CreateCheckBoxColumn("View", "ÚÑÖ"));
            dataGridView1.Columns.Add(CreateCheckBoxColumn("Add", "ÅÖÇİÉ"));
            dataGridView1.Columns.Add(CreateCheckBoxColumn("Edit", "ÊÚÏíá"));
            dataGridView1.Columns.Add(CreateCheckBoxColumn("Delete", "ÍĞİ"));
            dataGridView1.Columns.Add(CreateCheckBoxColumn("Print", "ØÈÇÚÉ"));

            //dataGridView1.Columns[0].DefaultCellStyle.BackColor = SystemColors.HotTrack;
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;

            // ÖÈØ ÇáÊäÓíŞÇÊ
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ÅÖÇİÉ ÈíÇäÇÊ
            AddRow("ÇáÚãáÇÁ", true, true, true, true, false/*, Color.Gray*/);
            AddRow("ÇáãæÑÏíä", true, false, false, false, false/*, Color.Azure*/);
            AddRow("ÇáãÎÒæä", false, false, false, false, false/*, Color.Red*/);
        }

        private DataGridViewCheckBoxColumn CreateCheckBoxColumn(string name, string headerText)
        {
            return new DataGridViewCheckBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                TrueValue = true,
                FalseValue = false,
                Width = 50
            };
        }

        private void AddRow(string screenName, bool view, bool add, bool edit, bool delete, bool print, Color? rowColor = null)
        {
            int rowIndex = dataGridView1.Rows.Add(screenName, view, add, edit, delete, print);

            // ÊÛííÑ áæä ÇáÕİ ÅĞÇ áÒã ÇáÃãÑ
            if (rowColor.HasValue)
                dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = rowColor.Value;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ÊÍŞŞ Ãä ÇáÎáíÉ åí ãä äæÚ CheckBox
            if (e.ColumnIndex > 0 && e.RowIndex >= 0) // ÊİÇÏí ÇáÚãæÏ ÇáÃæá
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);

                // ÊÍÏíÏ ãæŞÚ ÇáÏÇÆÑÉ
                int size = Math.Min(e.CellBounds.Width, e.CellBounds.Height) - 4;
                int x = e.CellBounds.X + (e.CellBounds.Width - size) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - size) / 2;

                using (Brush backgroundBrush = new SolidBrush(e.Value != null && (bool)e.Value ? Color.LightGreen : e.CellStyle.BackColor))
                using (Pen borderPen = new Pen(Color.Black, 1))
                {
                    // ÑÓã ÇáÎáİíÉ ÇáÏÇÆÑíÉ
                    e.Graphics.FillEllipse(backgroundBrush, x, y, size, size);

                    // ÑÓã ÍÏæÏ ÇáÏÇÆÑÉ
                    e.Graphics.DrawEllipse(borderPen, x, y, size, size);

                    // ÑÓã ÚáÇãÉ "ÕÍ" ÅĞÇ ßÇäÊ ÇáŞíãÉ True
                    if (e.Value != null && (bool)e.Value)
                    {
                        using (Pen checkPen = new Pen(Color.Green, 2))
                        {
                            e.Graphics.DrawLine(checkPen, x + size / 4, y + size / 2, x + size / 2, y + size * 3 / 4);
                            e.Graphics.DrawLine(checkPen, x + size / 2, y + size * 3 / 4, x + size * 3 / 4, y + size / 4);
                        }
                    }
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // ÇáÊÍŞŞ ãä Ãä ÇáÚãæÏ ÇáÃæá åæ ÇáãÓÊåÏİ
            {
                // ÊÍÏíÏ ßÇãá ÇáÕİ
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            // ÊÍŞŞ Ãä ÇáÎáíÉ ÇáÊí Êã ÇáäŞÑ ÚáíåÇ áíÓÊ ãä ÇáÚäÇæíä
            else if (e.RowIndex >= 0 && e.ColumnIndex > 0) // ÊİÇÏí ÇáÚãæÏ ÇáÃæá
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // ÚßÓ ÇáŞíãÉ ÇáÍÇáíÉ ááÎáíÉ
                if (cell.Value == null || !(bool)cell.Value)
                {
                    cell.Value = true; // ÅĞÇ áã Êßä ãÍÏÏÉ¡ ÍÏÏåÇ
                }
                else
                {
                    cell.Value = false; // ÅĞÇ ßÇäÊ ãÍÏÏÉ¡ Şã ÈÅáÛÇÁ ÇáÊÍÏíÏ
                }

                // ÊÍÏíË DataGridView
                dataGridView1.InvalidateCell(cell);
            }
        }
    }
}
