
namespace WinFormsGridView
{
    public partial class TreeForm : Form
    {
        public TreeForm()
        {
            InitializeComponent();
            InitializeTreeGridView();
        }

        private void TreeForm_Load(object sender, EventArgs e)
        {

        }

        private List<TreeNodeData> _treeData; // متغير لتخزين الشجرة

        private void InitializeTreeGridView()
        {
            // إعداد DataGridView
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // إعداد العمود الأول
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("ScreenName", "الشاشات");
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // تحميل البيانات
            _treeData = GetTreeData();
            PopulateTreeData(_treeData);
        }

        private void PopulateTreeData(List<TreeNodeData> treeData)
        {
            dataGridView1.Rows.Clear();

            foreach (var node in treeData)
            {
                AddTreeNode(node);
            }
        }

        private void AddTreeNode(TreeNodeData node)
        {
            // حساب المسافة البادئة بناءً على المستوى
            string indentedName = (node.ChildNodes.Count > 0 ? (node.IsExpanded ? "- " : "+ ") : "  ") +
                                  new string(' ', node.Level * 4) +
                                  node.ScreenName;

            // إضافة الصف
            int rowIndex = dataGridView1.Rows.Add(indentedName);
            dataGridView1.Rows[rowIndex].Tag = node; // تخزين العقدة في Tag للرجوع إليها لاحقًا

            // إذا كانت العقدة موسعة، أضف العقد الفرعية
            if (node.IsExpanded)
            {
                foreach (var child in node.ChildNodes)
                {
                    AddTreeNode(child);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // التحقق من العمود الأول
            {
                var node = dataGridView1.Rows[e.RowIndex].Tag as TreeNodeData;
                if (node != null && node.ChildNodes.Count > 0)
                {
                    // تبديل حالة التوسيع
                    node.IsExpanded = !node.IsExpanded;

                    // تحديث الصفوف مباشرة باستخدام الشجرة المحفوظة
                    PopulateTreeData(_treeData);
                }
            }
        }

        private List<TreeNodeData> GetTreeData()
        {
            return new List<TreeNodeData>
            {
                new TreeNodeData
                {
                    ScreenName = "العملاء",
                    Level = 0,
                    ChildNodes = new List<TreeNodeData>
                    {
                        new TreeNodeData { ScreenName = "إضافة عميل", Level = 1 },
                        new TreeNodeData 
                        { 
                            ScreenName = "عرض العملاء", 
                            Level = 1 ,
                            ChildNodes = new List<TreeNodeData>
                            {
                                new TreeNodeData { ScreenName = "عميل اول", Level = 2 },
                                new TreeNodeData { ScreenName = "عميل ثاني", Level = 2 },
                            }
                        },                        
                    }
                },
                new TreeNodeData
                {
                    ScreenName = "الموردين",
                    Level = 0,
                    ChildNodes = new List<TreeNodeData>
                    {
                        new TreeNodeData { ScreenName = "إضافة مورد", Level = 1 },
                        new TreeNodeData { ScreenName = "عرض الموردين", Level = 1 }
                    }
                }
            };
        }
    }

    class TreeNodeData
    {
        public string ScreenName { get; set; } = "";
        public int Level { get; set; } = 0; // المستوى (0 = رئيسي، 1 = فرعي، وهكذا)
        public bool IsExpanded { get; set; } = false; // حالة التوسيع
        public List<TreeNodeData> ChildNodes { get; set; } = new List<TreeNodeData>(); // العقد الفرعية
    }
}
