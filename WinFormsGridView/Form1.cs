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
            // ��� ���������
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ����� ������
            List<Rowtype> data = new List<Rowtype>
                {
                    new Rowtype() { screenName= "�������",canView = true,canAdd = false,canEdit= true,canDelete = false,canPrint = false },
                    new Rowtype() { screenName= "��������",canView = false,canAdd = true,canEdit= false,canDelete = true,canPrint = false },
                    new Rowtype() { screenName= "�������",canView = false,canAdd = false,canEdit= true,canDelete = false,canPrint = false },
                };
            dataGridView1.DataSource = data;
            dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            //dataGridView1.Columns[0].DefaultCellStyle.BackColor = SystemColors.HotTrack;
            dataGridView1.Columns[0].HeaderText = "�������";
            dataGridView1.Columns[1].HeaderText = "���";
            dataGridView1.Columns[2].HeaderText = "�����";
            dataGridView1.Columns[3].HeaderText = "�����";
            dataGridView1.Columns[4].HeaderText = "���";
            dataGridView1.Columns[5].HeaderText = "�����";
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ���� �� ������ �� �� ��� CheckBox
            if (e.ColumnIndex > 0 && e.RowIndex >= 0) // ����� ������ �����
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);

                // ����� ���� �������
                int size = Math.Min(e.CellBounds.Width, e.CellBounds.Height) - 4;
                int x = e.CellBounds.X + (e.CellBounds.Width - size) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - size) / 2;

                using (Brush backgroundBrush = new SolidBrush(e.Value != null && (bool)e.Value ? Color.Green : e.CellStyle.BackColor))
                using (Pen borderPen = new Pen(Color.Black, 1))
                {
                    // ��� ������� ��������
                    e.Graphics.FillEllipse(backgroundBrush, x, y, size, size);

                    // ��� ���� �������
                    e.Graphics.DrawEllipse(borderPen, x, y, size, size);

                    // ��� ����� "��" ��� ���� ������ True
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
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // ������ �� �� ������ ����� �� ��������
            {
                // ����� ���� ����
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            // ���� �� ������ ���� �� ����� ����� ���� �� ��������
            else if (e.RowIndex >= 0 && e.ColumnIndex > 0) // ����� ��� ������� ������
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // ���� �� ������ ����� ��� ���� ������
                if (cell.Value is bool currentValue)
                {
                    // ��� ������
                    cell.Value = !currentValue;

                    // ����� ��� ������
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
