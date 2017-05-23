using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiagnosticSystem
{
    class Classification
    {
        public static bool getValByRowIndexAndColName(DataTable table, int RowIndex, string ColName, ref double Value)
        {
            for (int ColIndex = 0; ColIndex < table.Columns.Count; ColIndex++)
            {
                if (table.Columns[ColIndex].ColumnName.Equals(ColName))
                {
                    Value = table.Rows[RowIndex].Field<double>(ColIndex);
                    return true;
                }
            }
            return false;
        }

        public static double classify(DataTable class_table, DataTable data_table, int RowIndex)
        {
            // def 
            double class_1_result = 0;
            String pattern = @"(x\d+)_(.+)";

            // iter by coef table
            foreach (DataRow row in class_table.Rows)
            {
                // gain coef name and value
                string var_name = row.Field<string>(0);

                if(var_name == null) continue;
                
                double var_koef = row.Field<double>(1);

                // search in data param with coef name
                double var_value = 0;
                if (getValByRowIndexAndColName(data_table, RowIndex, var_name, ref var_value))
                {
                    class_1_result += var_koef * var_value;
                    continue;
                }

                // check if this is nonlinear arg
                double arg_value = 1;
                Match m = Regex.Match(var_name, pattern);
                if (m.Success)
                {
                    string nl_var_name = m.Groups[1].Value;
                    string ml_arg = m.Groups[2].Value;

                    if(getValByRowIndexAndColName(data_table, RowIndex, nl_var_name, ref arg_value))
                    {
                        if (ml_arg.Equals("1_x"))
                            arg_value = (1 / arg_value);
                        if (ml_arg.Equals("xx"))
                            arg_value = Math.Pow(arg_value, 2);
                        if (ml_arg.Equals("xxx"))
                            arg_value = Math.Pow(arg_value, 3);
                        if (ml_arg.Equals("sqrt"))
                            arg_value = Math.Sqrt(arg_value);
                        if (ml_arg.Equals("ln"))
                            arg_value = Math.Log(arg_value);
                        if (ml_arg.Equals("exp"))
                            arg_value = Math.Exp(arg_value);
                    }
                }

                class_1_result += var_koef * arg_value;
            }

            double class_1_p = 1 / (1 + Math.Exp(-class_1_result));

            bool class_bool_result = true;

            if (class_1_p >= 0.5)
                class_bool_result = false;

            //Console.WriteLine("{0} Coef 1 z = {1} p = {2} class = {3}", 
            //    RowIndex, 
            //    class_1_result, 
            //    class_1_p, 
            //    class_bool_result);

            return class_1_p;
        }
    }
}
