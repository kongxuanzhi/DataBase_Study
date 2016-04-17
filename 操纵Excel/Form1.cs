using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace 操纵Excel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static void ExportToExcel(DataSet dataSet, string fileName)
        {
            if (dataSet.Tables.Count == 0)
            {
                throw new Exception("DataSet中没有任何可导出的表。");
            }

            Microsoft.Office.Interop.Excel.Application excelApplication = new Microsoft.Office.Interop.Excel.Application();
            excelApplication.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook workbook = excelApplication.Workbooks.Add(Missing.Value);

            foreach (System.Data.DataTable dt in dataSet.Tables)
            {
                Microsoft.Office.Interop.Excel.Worksheet lastWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(workbook.Worksheets.Count);
                Microsoft.Office.Interop.Excel.Worksheet newSheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(Type.Missing, lastWorksheet, Type.Missing, Type.Missing);

                newSheet.Name = dt.TableName;

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    newSheet.Cells[1, col + 1] = dt.Columns[col].ColumnName;
                }

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        newSheet.Cells[row + 2, col + 1] = dt.Rows[row][col].ToString();
                    }
                }
            }

            ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1)).Delete();
            ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1)).Delete();
            ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1)).Delete();
            ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1)).Activate();

            try
            {
                workbook.Close(true, fileName, System.Reflection.Missing.Value);
            }
            catch (Exception e)
            {
                throw e;
            }

            excelApplication.Quit();

            KillExcel();
        }

        private static void KillExcel()
        {
            Process[] excelProcesses = Process.GetProcessesByName("EXCEL");
            DateTime startTime = new DateTime();

            int processId = 0;
            for (int i = 0; i < excelProcesses.Length; i++)
            {
                if (startTime < excelProcesses[i].StartTime)
                {
                    startTime = excelProcesses[i].StartTime;
                    processId = i;
                }
            }

            if (excelProcesses[processId].HasExited == false)
            {
                excelProcesses[processId].Kill();
            }
        }
    }
}