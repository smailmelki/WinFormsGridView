using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsGridView
{
    public partial class TreeForm : Form
    {
        public TreeForm()
        {
            InitializeComponent();
        }

        // متغير لتخزين بيانات الشجرة
        private List<Header> _treeData;

        private void TreeForm_Load(object sender, EventArgs e)
        {
            // إعداد الشجرة عند تحميل النموذج
            InitializeTreeGridView();
        }

        // إعداد DataGridView لعرض الشجرة
        private void InitializeTreeGridView()
        {
            // إعدادات عامة للـ DataGridView
            dataGridView1.AllowUserToAddRows = false; // منع إضافة صفوف يدويًا
            dataGridView1.RowHeadersVisible = false; // إخفاء الأعمدة الجانبية
            dataGridView1.AllowUserToResizeRows = false; // منع تغيير حجم الصفوف
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically; // منع التحرير اليدوي
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ملء الأعمدة تلقائيًا

            // إنشاء أعمدة DataGridView
            dataGridView1.Columns.Clear();
            // عمود لعرض أسماء الشاشات
            dataGridView1.Columns.Add("Num", "الرقم");
            dataGridView1.Columns.Add("Name", "الاسم");
            dataGridView1.Columns.Add("Date", "التاريخ");
            dataGridView1.Columns.Add("Total", "المجموع");
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // ضبط محاذاة النص

            // تحميل البيانات الأولية للشجرة
            _treeData = GetTreeData();
            PopulateTreeData(_treeData);
        }

        // ملء الشجرة بالبيانات
        private void PopulateTreeData(List<Header> treeData)
        {
            // تفريغ الصفوف الحالية
            dataGridView1.Rows.Clear();

            // إضافة العقد الرئيسية للشجرة
            foreach (var node in treeData)
            {
                AddHeaderNode(node);
            }
        }

        // إضافة عقدة رئيسية (Header) إلى DataGridView
        private void AddHeaderNode(Header node)
        {
            // إنشاء صف جديد
            int rowIndex = dataGridView1.Rows.Add(node.Code, node.Name, node.date, node.Total);
            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Rows[rowIndex].DefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold); // تغيير الخط للعقد الرئيسية
            var row = dataGridView1.Rows[rowIndex];
            row.Tag = node; // تخزين بيانات العقدة في الصف

            // إذا كانت العقدة موسعة، أضف العقد الفرعية
            if (node.IsExpanded)
            {
                int index = dataGridView1.Rows.Add("Code", "Item", "Price", "Qty");
                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.LightGreen;
                dataGridView1.Rows[index].DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold); // تغيير الخط للعقد الرئيسية

                foreach (var child in node.DetailsList)
                {
                    AddDetailNode(child);
                }
            }
        }

        // إضافة عقدة فرعية (Detailes) إلى DataGridView
        private void AddDetailNode(Detailes node)
        {
            // إنشاء صف جديد
            int index = dataGridView1.Rows.Add(node.Code, node.Item, node.Price, node.Qty);
            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.White;
        }

        // حدث النقر على الخلية
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // التحقق من أن النقر في العمود الأول (رمز التوسيع/الإغلاق)
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var node = row.Tag as Header;

                if (node != null && node.DetailsList.Count > 0)
                {
                    // تبديل حالة التوسيع
                    node.IsExpanded = !node.IsExpanded;

                    // تحديث الصفوف مباشرة باستخدام البيانات المخزنة
                    PopulateTreeData(_treeData);
                }
            }
        }

        // إنشاء بيانات الشجرة كمثال
        private List<Header> GetTreeData()
        {
            var data = new List<Header>
            {
                new Header
                {
                    Code = "001",
                    Name = "Ali",
                    date = DateOnly.Parse("1/1/2024"),
                    DetailsList = new List<Detailes>
                    {
                        new Detailes { Code = "123", Item = "Lait", Price = 10, Qty = 2 },
                        new Detailes { Code = "124", Item = "Coffee", Price = 15, Qty = 3 },
                    }
                },
                new Header
                {
                    Code = "002",
                    Name = "Ahmed",
                    date = DateOnly.Parse("2/1/2024"),
                    DetailsList = new List<Detailes>
                    {
                        new Detailes { Code = "125", Item = "Sugar", Price = 35, Qty = 1 },
                        new Detailes { Code = "126", Item = "Tomato", Price = 25, Qty = 4 },
                    }
                }
            };

            // حساب المجموع تلقائيًا لكل عقدة رئيسية
            foreach (var header in data)
            {
                header.Total = header.DetailsList.Sum(detail => detail.Price * detail.Qty);
            }

            return data;
        }

    }

    // تعريف كلاس Header للعقد الرئيسية
    class Header
    {
        public int Level { get; set; } = 0; // المستوى (0 = رئيسي)
        public bool IsExpanded { get; set; } = false; // حالة التوسيع
        public string Code { get; set; } = ""; // الكود
        public string Name { get; set; } = ""; // اسم طرف التعامل
        public DateOnly date { get; set; } // التاريخ
        public double Total { get; set; } // المجموع
        public List<Detailes> DetailsList { get; set; } = new List<Detailes>(); // قائمة العقد الفرعية
    }

    // تعريف كلاس Detailes للعقد الفرعية
    class Detailes
    {
        public int Level { get; set; } = 1; // المستوى
        public string Code { get; set; } // الكود
        public string Item { get; set; } // العنصر
        public double Price { get; set; } // السعر
        public double Qty { get; set; } // الكمية
    }
}
