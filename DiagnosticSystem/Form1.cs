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

        // table for storing user inputed data in Work mode
        DataTable userDataTable;
        // *hardcode* 4 Tables for each class: 1_234, 2_134, 3_124, 4_123
        DataSet classificators;

        DataSet data, work_data;

        

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

        private DataSet getClassificatorsDataSet()
        {
            // todo: retrieve path from app configurations
            string FilePathVladlen = "d:/Documents/GitHub/diagnostic-system-csharp/classificators_3.xls";
            string FilePathYura = "d:/Documents/GitHub/diagnostic-system-csharp/classificators_2.xls";

            return readDataSetFromExcel(FilePathVladlen, false);
        }

        private void initUserDataTable()
        {
            userDataTable = new DataTable();


            DataColumn column;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "N";
            userDataTable.Columns.Add(column);

            for (int i = 0; i < 69; i++)
            {
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");
                column.ColumnName = "x" + i.ToString();
                userDataTable.Columns.Add(column);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // load classificators
            classificators = getClassificatorsDataSet();

            initUserDataTable();
            
            // mount user table to grid view for ability manual editing in Work mode
            dgvWorkMode.DataSource = userDataTable;
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

        private int getColIndexByColName(DataTable table, string ColName)
        {
            for (int ColIndex = 0; ColIndex < table.Columns.Count; ColIndex++)
            {
                if (table.Columns[ColIndex].ColumnName.Equals(ColName))
                {
                    return ColIndex;
                }
            }
            return -1;
        }

        private double classification(DataTable data_table)
        {
            int[] statistic_success = { 0, 0, 0, 0 };

            int res_col_index = -1;

            if(!data_table.Columns.Contains("k-result"))
                data_table.Columns.Add("k-result");

            res_col_index = getColIndexByColName(data_table, "k-result");
            

            double res_1_123, res_2_134, res_3_124, res_4_123;
            for (int RowIndex = 0; RowIndex < data_table.Rows.Count; RowIndex++)
            {
                res_1_123 = Classification.classify(classificators.Tables["1_234"], data_table, RowIndex);
                res_2_134 = Classification.classify(classificators.Tables["2_134"], data_table, RowIndex);
                res_3_124 = Classification.classify(classificators.Tables["3_124"], data_table, RowIndex);
                res_4_123 = Classification.classify(classificators.Tables["4_123"], data_table, RowIndex);

                int diagnos = 0;

                if (res_1_123 < res_2_134 &&
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



                data_table.Rows[RowIndex].SetField<int>(res_col_index, diagnos);
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

            return total_success;
        }

        private double classification_da(DataTable data_table)
        {
            int[] statistic_success = { 0, 0, 0, 0 };

            int res_col_index = -1;

            if (!data_table.Columns.Contains("k-result"))
                data_table.Columns.Add("k-result");

            res_col_index = getColIndexByColName(data_table, "k-result");


            double res_1_123, res_2_134, res_3_124, res_4_123;
            for (int RowIndex = 0; RowIndex < data_table.Rows.Count; RowIndex++)
            {
                res_1_123 = Classification.classify_da(classificators.Tables["1_234"], data_table, RowIndex);
                res_2_134 = Classification.classify_da(classificators.Tables["2_134"], data_table, RowIndex);
                res_3_124 = Classification.classify_da(classificators.Tables["3_124"], data_table, RowIndex);
                res_4_123 = Classification.classify_da(classificators.Tables["4_123"], data_table, RowIndex);

                int diagnos = 0;

                if (res_1_123 > res_2_134 &&
                   res_1_123 > res_3_124 &&
                   res_1_123 > res_4_123) diagnos = 1;

                if (res_2_134 > res_1_123 &&
                   res_2_134 > res_3_124 &&
                   res_2_134 > res_4_123) diagnos = 2;

                if (res_3_124 > res_1_123 &&
                   res_3_124 > res_2_134 &&
                   res_3_124 > res_4_123) diagnos = 3;

                if (res_4_123 > res_1_123 &&
                   res_4_123 > res_2_134 &&
                   res_4_123 > res_3_124) diagnos = 4;



                data_table.Rows[RowIndex].SetField<int>(res_col_index, diagnos);
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

            return total_success;
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            //DataTable data_table = data.Tables["mitral"];
            DataTable data_table = (DataTable)dataGridView.DataSource;

            double total_success = classification(data_table);

            lblTotalSuccessTest.Text = String.Format("{0:P}", total_success);


        }

        private void btnClassifyWork_Click(object sender, EventArgs e)
        {
            DataTable data_table = (DataTable)dgvWorkMode.DataSource;

            double total_success = classification_da(data_table);

            //lblTotalSuccessTest.Text = String.Format("{0:P}", total_success);
        }

        private void cboSheetWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvWorkMode.DataSource = work_data.Tables[cboSheetWork.SelectedIndex];
        }

        private void PasteClipboard(DataTable myDataTable, bool isColumnAdd = false, bool isRowsClear = false)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {

                if (isRowsClear && myDataTable.Rows.Count > 0)
                    myDataTable.Rows.Clear();

                if (isColumnAdd && myDataTable.Columns.Count > 0)
                    myDataTable.Columns.Clear();

                bool columnsAdded = false;
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    if (isColumnAdd && !columnsAdded)
                    {
                        DataColumn column;
                        for (int i = 0; i < pastedRowCells.Length; i++)
                        {
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.IntDouble");
                            column.ColumnName = pastedRowCells[i];

                            myDataTable.Columns.Add(column);
                        }

                        columnsAdded = true;
                        continue;
                    }

                    if(pastedRowCells.Length > myDataTable.Columns.Count)
                        myDataTable.Rows.Add(pastedRowCells.Take(myDataTable.Columns.Count).ToArray());
                    else
                        myDataTable.Rows.Add(pastedRowCells);
                }
            }
        }

        private void dgvWorkMode_KeyUp(object sender, KeyEventArgs e)
        {
            //if user clicked Shift+Ins or Ctrl+V (paste from clipboard)
            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V))
            {
                PasteClipboard(userDataTable);
            }
        }

        private void btnOpenWork_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xls", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    work_data = readDataSetFromExcel(ofd.FileName);

                    cboSheetWork.Items.Clear();
                    foreach (DataTable dt in work_data.Tables)
                        cboSheetWork.Items.Add(dt.TableName);

                }
            }
        }
    }
}
