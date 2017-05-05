using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiagnosticSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSet result;

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void btnOpen_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter="Excel Workbook|*.xls", ValidateNames = true})
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                    IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
                    reader.IsFirstRowAsColumnNames = true;
                    result = reader.AsDataSet();
                    cboSheet.Items.Clear();
                    foreach (DataTable dt in result.Tables)
                        cboSheet.Items.Add(dt.TableName);
                    reader.Close();
                }
            }
        }

        private void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView.DataSource = result.Tables[cboSheet.SelectedIndex];
        }
    }
}
