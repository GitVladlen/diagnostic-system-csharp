using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        DataSet data;

        DataSet classificators;

        // ----------------------------------------------------------------------------------
        private DataSet readDataSetFromExcel(string FileName, bool IsFirstRowAsColumnNames = true)
        {
            FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
            reader.IsFirstRowAsColumnNames = IsFirstRowAsColumnNames;
            DataSet result = reader.AsDataSet();
            reader.Close();

            return result;
        }

        // ----------------------------------------------------------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            data = readDataSetFromExcel("d:/Documents/GitHub/diagnostic-system-csharp/test-data.xls");
            classificators = readDataSetFromExcel("d:/Documents/GitHub/diagnostic-system-csharp/classificators.xls", false);
        }

        // ----------------------------------------------------------------------------------

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter="Excel Workbook|*.xls", ValidateNames = true})
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    data = readDataSetFromExcel(ofd.FileName);

                    cboSheet.Items.Clear();
                    foreach (DataTable dt in data.Tables)
                        cboSheet.Items.Add(dt.TableName);

                }
            }
        }

        // ----------------------------------------------------------------------------------

        private void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView.DataSource = data.Tables[cboSheet.SelectedIndex];
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            DataTable data_table = data.Tables["mitral"];

            int[] statistic_success = { 0, 0, 0, 0 };

            double res_1_123, res_2_134, res_3_124, res_4_123;
            for (int RowIndex = 0; RowIndex < data_table.Rows.Count; RowIndex++)
            {
                res_1_123 = Classification.classify(classificators.Tables["1_234"], data_table, RowIndex);
                res_2_134 = Classification.classify(classificators.Tables["2_134"], data_table, RowIndex);
                res_3_124 = Classification.classify(classificators.Tables["3_124"], data_table, RowIndex);
                res_4_123 = Classification.classify(classificators.Tables["4_123"], data_table, RowIndex);

                int diagnos = 0;
                
                if(res_1_123 < res_2_134 &&
                   res_1_123 < res_3_124 &&
                   res_1_123 < res_4_123) diagnos = 1;

                if (res_2_134 < res_1_123 &&
                   res_2_134 < res_3_124 &&
                   res_2_134 < res_4_123) diagnos = 2;

                if (res_3_124 < res_1_123 &&
                   res_3_124 < res_2_134 &&
                   res_3_124 < res_4_123) diagnos = 3;

                if (res_4_123 < res_1_123 &&
                   res_4_123 < res_2_134 &&
                   res_4_123 < res_3_124) diagnos = 4;

                double true_diagnos = 0;
                Classification.getValByRowIndexAndColName(data_table, RowIndex, "k", ref true_diagnos);

                int true_diagnos_int = Convert.ToInt16(true_diagnos);

                bool isSucces = diagnos == true_diagnos_int;

                if (isSucces)
                    statistic_success[true_diagnos_int - 1] += 1;

                Console.WriteLine("{0}: 1={1}, 2={2}, 3={3}, 4={4}, diagnos={5}, true_dignos={6} >> {7}", 
                    RowIndex, 
                    res_1_123, 
                    res_2_134, 
                    res_3_124, 
                    res_4_123,
                    diagnos,
                    true_diagnos_int,
                    isSucces);
            }

            double total_success = statistic_success.Sum() / Convert.ToDouble(data_table.Rows.Count);

            Console.WriteLine("Total Success = {0}", total_success);
        }
    }
}
