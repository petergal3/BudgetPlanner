using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MountainFinance
{
    class ExcelManager
    {
        private LoginManager lm = LoginManager.GetSingleton();
        private DataManager dm = DataManager.GetSingleton();
        string sFileName;
        public OpenFileDialog OpenFileDialog1;
        private static int userId = -1;
        public string path = "";

        public void CreateSampleExcel(){
            Excel.Application xlApp;
            object misValue = System.Reflection.Missing.Value;
            Excel.Workbook xlWorkBook;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);


            xlWorkBook = xlApp.Workbooks.Add(misValue);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "transactions";
            xlWorkSheet.Cells[1, 1] = "category";
            xlWorkSheet.Cells[1, 2] = "type";
            xlWorkSheet.Cells[1, 3] = "description";
            xlWorkSheet.Cells[1, 4] = "amount";
            xlWorkSheet.Cells[1, 5] = "date";

            var xlWorkSheet1 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue);
            xlWorkSheet1.Name = "saving_goal";
            xlWorkSheet1.Cells[1, 1] = "goal";
            xlWorkSheet1.Cells[1, 2] = "date";

            var xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.Add(misValue);
            xlWorkSheet2.Name = "saving";
            xlWorkSheet2.Cells[1, 1] = "amount";
            xlWorkSheet2.Cells[1, 2] = "date";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\sample_excel";
            xlWorkBook.SaveAs(path);
            MessageBox.Show("Excel file created , it is on your desk!");
        
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
        }


        public void InsertExcelToDb()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            if (sFileName != null)
            {

                if (sFileName.Trim() != "")
                {
                    readExcel(sFileName, "transactions");
                    readExcel(sFileName, "saving_goal");
                    readExcel(sFileName, "saving");
                }
            }

            void readExcel(string sFile, string sheet)
            {
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(sFile);
                xlWorkSheet = xlWorkBook.Worksheets[sheet];



                int iRow, iCol = 2;
                for (iRow = 2; iRow <= xlWorkSheet.Rows.Count; iRow++)  // START FROM THE SECOND ROW.
                {
                    if (xlWorkSheet.Cells[iRow, 1].value == null)
                    {
                        break;      // BREAK LOOP.
                    }
                    else
                    {

                        switch (sheet)
                        {
                            case "transactions":

                                if (xlWorkSheet.Cells[iRow, 2].value == "income")
                                {
                                    dm.SqlInsertion(new Income(xlWorkSheet.Cells[iRow, 1].value, xlWorkSheet.Cells[iRow, 3].value, Convert.ToInt32(xlWorkSheet.Cells[iRow, 4].value),
                                    Convert.ToDateTime(xlWorkSheet.Cells[iRow, 5].value), 1), LoginManager.GetSingleton());

                                }
                                else if (xlWorkSheet.Cells[iRow, 2].value == "expense")
                                {
                                   dm.SqlInsertion(new Expense(xlWorkSheet.Cells[iRow, 1].value, xlWorkSheet.Cells[iRow, 3].value, Convert.ToInt32(xlWorkSheet.Cells[iRow, 4].value),
                                   Convert.ToDateTime(xlWorkSheet.Cells[iRow, 5].value), 1), LoginManager.GetSingleton());

                                }

                                break;
                            case "saving_goal":
                                dm.SqlInsertionSavingGoal(Convert.ToInt32(xlWorkSheet.Cells[iRow, 1].value), Convert.ToDateTime(xlWorkSheet.Cells[iRow, 2].value), LoginManager.GetSingleton());

                                break;
                            case "saving":
                                dm.SqlInsertion(new Saving(Convert.ToInt32(xlWorkSheet.Cells[iRow, 1].value), Convert.ToDateTime(xlWorkSheet.Cells[iRow, 2].value), 1), LoginManager.GetSingleton());

                                break;
                            default:
                                break;
                        }

                    }
                }

                xlWorkBook.Close();
                xlApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            }



        }

        public void ImportDataFromExcel()
        {

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;

            OpenFileDialog1 = new OpenFileDialog();
            // OPEN FILE DIALOG AND SELECT AN EXCEL FILE.
            OpenFileDialog1.Title = "Excel File to Edit";
            OpenFileDialog1.FileName = "";
            OpenFileDialog1.Filter = "Excel File|*.xlsx;*.xls";
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sFileName = OpenFileDialog1.FileName;
                xlWorkBook = xlApp.Workbooks.Open(sFileName);
                path = xlWorkBook.Name;
                xlWorkBook.Close();
                xlApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            }
        }

        public void ExportToExcel(string startdate, string enddate, string table)
        {
            Excel.Application xlApp;
            object misValue = System.Reflection.Missing.Value;
            Excel.Workbook xlWorkBook;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            bool wrongValue = false;
            switch (table)
            {
                case "all":
                    FillExcelSheet(xlWorkBook, startdate, enddate, "transactions", 1);
                    FillExcelSheet(xlWorkBook, startdate, enddate, "saving", 2);
                    FillExcelSheet(xlWorkBook, startdate, enddate, "saving_goal", 3);
                    break;
                case "saving":
                    FillExcelSheet(xlWorkBook, startdate, enddate, "saving", 1);
                    break;
                case "saving goal":
                    FillExcelSheet(xlWorkBook, startdate, enddate, "saving_goal", 1);
                    break;
                case "transactions":
                    FillExcelSheet(xlWorkBook, startdate, enddate, "transactions", 1);
                    break;
                default:
                    wrongValue = true;
                    MessageBox.Show("Invalid datatype value");
                    break;
            }
            if (!wrongValue)
            {
                int n = 1;
                string p = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\MountainFinance" + $"{n}" + ".xlsx";

                while (File.Exists(p))
                {
                    n++;
                    p = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\MountainFinance" + $"{n}" + ".xlsx";

                }
                xlWorkBook.SaveAs(p);
                MessageBox.Show("Excel file created , you can find the file on your desktop");
            }
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
        private void FillExcelSheet(Excel.Workbook xlWorkBookbase, string startdate, string enddate, string table, int worksheetNum)
        {
            string data = null;
            int i = 0;
            int j = 0;


            Excel.Worksheet xlWorkSheet;
            Excel.Workbook xlWorkBook = xlWorkBookbase;

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add();
            xlWorkSheet.Name = table;
            
            DataTable dt = dm.SqlGetData(startdate, enddate, table);

            int n = 1;
            foreach (DataColumn column in dt.Columns)
            {
                xlWorkSheet.Cells[1, n] = column.ColumnName;
                n++;
            }

            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    data = dt.Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 2, j + 1] = data;
                }
            }

            releaseObject(xlWorkSheet);
        }


        private void releaseObject(object obj)
        {

            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }

        }
    }
}
