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
            try
            {
                FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
                reader.IsFirstRowAsColumnNames = IsFirstRowAsColumnNames;
                DataSet result = reader.AsDataSet();
                reader.Close();

                return result;
            }
            catch (System.IO.IOException eIO)
            {
                MessageBox.Show(
                    "Помилка відкриття файлу: " + FileName,
                    "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        // ----------------------------------------------------------------------------------

        private DataSet getClassificatorsDataSet()
        {
            // todo: retrieve path from app configurations
            string FilePath = "./classificators_5.xls";

            return readDataSetFromExcel(FilePath, false);
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

            modify_col_names(userDataTable);
        }

        private void modify_col_names(DataTable table)
        {
            DataSet settings = readDataSetFromExcel("./settings.xls");

            if (settings == null)
            {
                Application.Exit();
                return;
            }

            DataTable var_names_table = settings.Tables["var_names"];

            foreach (DataColumn col in table.Columns)
            { 
                foreach (DataRow row in var_names_table.Rows)
                {
                    string var_name = row.Field<string>(0);
                    string var_descr = row.Field<string>(1);

                    if (col.ColumnName.Equals(var_name))
                    {
                        if (!(var_descr == null))
                        {
                            col.ColumnName += "\n" + var_descr;
                        }
                        break;
                    }
                }
            }
        }

        private void modify_result_codes(DataTable table)
        {
            DataSet settings = readDataSetFromExcel("./settings.xls");

            if (settings == null)
            {
                Application.Exit();
                return;
            }

            DataTable result_names_table = settings.Tables["result"];

            int res_col_index = getColIndexByColName(table, "k-result");

            foreach(DataRow source_row in table.Rows)
            {
                if (source_row.RowState.Equals(DataRowState.Deleted))
                    continue;

                //double cur_value = Convert.ToDouble(source_row.Field<string>("k-result"));
                double cur_value = Convert.ToDouble(source_row.Field<string>(res_col_index));

                foreach (DataRow row in result_names_table.Rows)
                {
                    double result_code = row.Field<double>(0);
                    string result_descr = row.Field<string>(1);

                    if (cur_value == result_code)
                    {
                        source_row.SetField<string>(res_col_index, result_descr);
                        break;
                    }
                }
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
            DataTable source_table = data.Tables[cboSheet.SelectedIndex];
            modify_col_names(source_table);
            dataGridView.DataSource = source_table;
        }

        private int getColIndexByColName(DataTable table, string ColName)
        {
            for (int ColIndex = 0; ColIndex < table.Columns.Count; ColIndex++)
            {
                if (table.Columns[ColIndex].ColumnName.StartsWith(ColName))
                {
                    return ColIndex;
                }
            }
            return -1;
        }

        private double classification_da(DataTable data_table)
        {
            int[] statistic_success = { 0, 0, 0, 0 };

            int res_col_index = -1;

            res_col_index = getColIndexByColName(data_table, "k-result");

            if (res_col_index != -1)
            {
                data_table.Columns.RemoveAt(res_col_index);
            }

            data_table.Columns.Add("k-result");

            res_col_index = getColIndexByColName(data_table, "k-result");

            data_table.Columns[res_col_index].DataType = System.Type.GetType("System.String");


            double res_1_123, res_2_134, res_3_124, res_4_123;
            for (int RowIndex = 0; RowIndex < data_table.Rows.Count; RowIndex++)
            {
                if (data_table.Rows[RowIndex].RowState.Equals(DataRowState.Deleted))
                    continue;

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

                if (isSucces && true_diagnos_int > 0)
                    statistic_success[true_diagnos_int - 1] += 1;

                //Console.WriteLine("{0}: 1={1}, 2={2}, 3={3}, 4={4}, diagnos={5}, true_dignos={6} >> {7}",
                //    RowIndex,
                //    res_1_123,
                //    res_2_134,
                //    res_3_124,
                //    res_4_123,
                //    diagnos,
                //    true_diagnos_int,
                //    isSucces);

            }

            modify_col_names(data_table);

            double total_success = statistic_success.Sum() / Convert.ToDouble(data_table.Rows.Count);

            Console.WriteLine("Total Success = {0}", total_success);

            return total_success;
        }

        private void classification_da_work(DataTable data_table)
        {
            int res_col_index = -1;

            res_col_index = getColIndexByColName(data_table, "k-result");

            if (res_col_index != -1)
            {
                data_table.Columns.RemoveAt(res_col_index);
            }

            data_table.Columns.Add("k-result");
            
            res_col_index = getColIndexByColName(data_table, "k-result");

            data_table.Columns[res_col_index].DataType = System.Type.GetType("System.String");

            double res_1_123, res_2_134, res_3_124, res_4_123;
            for (int RowIndex = 0; RowIndex < data_table.Rows.Count; RowIndex++)
            {
                if (data_table.Rows[RowIndex].RowState.Equals(DataRowState.Deleted))
                    continue;

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

                //Console.WriteLine("{0}: 1={1}, 2={2}, 3={3}, 4={4}, diagnos={5}",
                //    RowIndex,
                //    res_1_123,
                //    res_2_134,
                //    res_3_124,
                //    res_4_123,
                //    diagnos);
            }
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count <= 1)
            {
                MessageBox.Show(
                    "Дані відсутні!",
                    "Попередження",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            DataTable data_table = (DataTable)dataGridView.DataSource;

            double total_success = classification_da(data_table);

            lblTotalSuccessTest.Text = String.Format("{0:P}", total_success);

            int res_col_index = getColIndexByColName(data_table, "k-result");

            modify_result_codes(data_table);
            modify_col_names(data_table);

            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[res_col_index];

            MessageBox.Show(
                    "Класифікація виконана\nВідсоток правильно класифікованих об'єктів: " + String.Format("{0:P}", total_success),
                    "Статус",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

        }

        private void btnClassifyWork_Click(object sender, EventArgs e)
        {
            if (dgvWorkMode.Rows.Count <= 1)
            {
                MessageBox.Show(
                    "Дані відсутні!",
                    "Попередження",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            DataTable data_table = (DataTable)dgvWorkMode.DataSource;

            classification_da_work(data_table);

            int res_col_index = getColIndexByColName(data_table, "k-result");

            modify_result_codes(data_table);
            modify_col_names(data_table);

            dgvWorkMode.CurrentCell = dgvWorkMode.Rows[0].Cells[res_col_index];

            MessageBox.Show(
                    "Класифікація виконана",
                    "Статус",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void cboSheetWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dgvWorkMode.DataSource = work_data.Tables[cboSheetWork.SelectedIndex];

            DataTable source_table = work_data.Tables[cboSheetWork.SelectedIndex];
            modify_col_names(source_table);
            dgvWorkMode.DataSource = source_table;
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

        private void dgvWorkMode_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            dgvWorkMode.Rows[e.RowIndex].ErrorText = "";
            double newDouble;

            // Don't try to validate the 'new row' until finished 
            // editing since there
            // is not any point in validating its initial value.
            if (dgvWorkMode.Rows[e.RowIndex].IsNewRow) { return; }

            int res_col_index = getColIndexByColName((DataTable)dgvWorkMode.DataSource, "k-result");
            if (e.ColumnIndex == res_col_index) { return; }

            if (!e.FormattedValue.ToString().Equals("") && !double.TryParse(e.FormattedValue.ToString(),
                out newDouble) )
            {
                e.Cancel = true;
                dgvWorkMode.Rows[e.RowIndex].ErrorText = "Значення комірки повинно бути числом";
            }
        }

        private void проПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
