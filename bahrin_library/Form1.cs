using ClosedXML.Excel;
using System.Data;
using System.Reflection;

namespace bahrin_library
{
    public partial class Form1 : Form
    {
        string A = "";
        public Form1()
        {
            InitializeComponent();
            callExcelFile();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataView? dv = dataGridView1.DataSource as DataView;
                if (dv != null)
                {
                    dv.RowFilter = A;
                }
                else
                {
                    MessageBox.Show("لا يوجد نتائج");
                }

                label2.Text = $"النتائج {dataGridView1.RowCount - 1}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void callExcelFile()
        {
            string exeFile = new Uri(Assembly.GetEntryAssembly().CodeBase).AbsolutePath;

            string exeDir = Path.GetDirectoryName(exeFile);
            string fullPath = Path.Combine(path1: exeDir, "Resources\\bahrin_lib.xlsx");
            Cursor.Current = Cursors.WaitCursor;

            DataTable dt = new DataTable();
            using (XLWorkbook wb = new XLWorkbook(fullPath))
            {
                bool isFirstRow = true;
                var rows = wb.Worksheet(1).RowsUsed();
                foreach (var row in rows)
                {
                    if (isFirstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString());
                        isFirstRow = false;

                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                            dt.Rows[dt.Rows.Count - 1][i++] = cell.Value.ToString();
                    }
                }
                dataGridView1.DataSource = dt.DefaultView;

                label2.Text = $"النتائج {dataGridView1.RowCount - 1}";

                Cursor.Current = Cursors.Default;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            /*            A = $"A like'%{textBox1.Text}%'" ?? $"B like'%{textBox1.Text}%'";*/
            A = $"A like'%{textBox1.Text}%' OR B like'%{textBox1.Text}%'";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
        }
    }
}