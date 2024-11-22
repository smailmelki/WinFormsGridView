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
            // ÷»ÿ «· ‰”Ìﬁ« 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ≈÷«›… »Ì«‰« 
            List<Rowtype> data = new List<Rowtype>
                {
                    new Rowtype() { screenName= "«·⁄„·«¡",canView = true,canAdd = false,canEdit= true,canDelete = false,canPrint = false },
                    new Rowtype() { screenName= "«·„Ê—œÌ‰",canView = false,canAdd = true,canEdit= false,canDelete = true,canPrint = false },
                    new Rowtype() { screenName= "«·„Œ“Ê‰",canView = false,canAdd = false,canEdit= true,canDelete = false,canPrint = false },
                };
            dataGridView1.DataSource = data;
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            //dataGridView1.Columns[0].DefaultCellStyle.BackColor = SystemColors.HotTrack;
            dataGridView1.Columns[0].HeaderText = "«·‘«‘« ";
            dataGridView1.Columns[1].HeaderText = "⁄—÷";
            dataGridView1.Columns[2].HeaderText = "«÷«›…";
            dataGridView1.Columns[3].HeaderText = " ⁄œÌ·";
            dataGridView1.Columns[4].HeaderText = "Õ–›";
            dataGridView1.Columns[5].HeaderText = "ÿ»«⁄…";
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //  Õﬁﬁ √‰ «·Œ·Ì… ÂÌ „‰ ‰Ê⁄ CheckBox
            if (e.ColumnIndex > 0 && e.RowIndex >= 0) //  ›«œÌ «·⁄„Êœ «·√Ê·
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);

                //  ÕœÌœ „Êﬁ⁄ «·œ«∆—…
                int size = Math.Min(e.CellBounds.Width, e.CellBounds.Height) - 4;
                int x = e.CellBounds.X + (e.CellBounds.Width - size) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - size) / 2;

                using (Brush backgroundBrush = new SolidBrush(e.Value != null && (bool)e.Value ? Color.Green : e.CellStyle.BackColor))
                using (Pen borderPen = new Pen(Color.Black, 1))
                {
                    // —”„ «·Œ·›Ì… «·œ«∆—Ì…
                    e.Graphics.FillEllipse(backgroundBrush, x, y, size, size);

                    // —”„ ÕœÊœ «·œ«∆—…
                    e.Graphics.DrawEllipse(borderPen, x, y, size, size);

                    // —”„ ⁄·«„… "’Õ" ≈–« ﬂ«‰  «·ﬁÌ„… True
                    if (e.Value != null && (bool)e.Value)
                    {
                        using (Pen checkPen = new Pen(Color.White, 2))
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
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // «· Õﬁﬁ „‰ √‰ «·⁄„Êœ «·√Ê· ÂÊ «·„” Âœ›
            {
                //  ÕœÌœ ﬂ«„· «·’›
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            //  Õﬁﬁ √‰ «·Œ·Ì… «· Ì  „ «·‰ﬁ— ⁄·ÌÂ« ·Ì”  „‰ «·⁄‰«ÊÌ‰
            else if (e.RowIndex >= 0 && e.ColumnIndex > 0) // «·‰ﬁ— ⁄·Ï «·√⁄„œ… «·√Œ—Ï
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                //  Õﬁﬁ √‰ «·Œ·Ì…  Õ ÊÌ ⁄·Ï ﬁÌ„… „‰ÿﬁÌ…
                if (cell.Value is bool currentValue)
                {
                    // ⁄ﬂ” «·ﬁÌ„…
                    cell.Value = !currentValue;

                    //  ÕœÌÀ ⁄—÷ «·Œ·Ì…
                    dataGridView1.InvalidateCell(cell);
                }
            }
        }
    }
    class Rowtype
    {
        public string screenName { get; set; } = "";
        public bool canView { get; set; }
        public bool canAdd { get; set; }
        public bool canEdit { get; set; }
        public bool canDelete { get; set; }
        public bool canPrint { get; set; }
    }
}
