using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data;
using System.Reflection;
using Microsoft.Win32;


namespace Mint.Code
{
    internal class XLSX
    {
        public static bool IsExcelInstalled()
        {
            try
            {
                string progId;
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(".xlsx"))
                {
                    if (key == null)
                    {
                        // .xlsx extension not registered
                        return false;
                    }
                    progId = key.GetValue(null) as string;
                }

                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(progId))
                {
                    if (key == null)
                    {
                        // ProgID not found
                        return false;
                    }
                    string appName = key.GetValue(null) as string;

                    // Check if the application name is Excel-related
                    return appName != null && appName.IndexOf("Excel", StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for Excel file association: {ex.Message}");
                return false;
            }
        }

        //private static void CompleteNamedRanges(Excel.Worksheet worksheet, Dictionary<string, object> namedRanges)
        private static void CompleteNamedRanges(dynamic worksheet, Dictionary<string, object> namedRanges)

        {
            foreach (var namedRange in namedRanges)
            {
                try
                {
                    // Find the named range
                    //Excel.Range range = null;
                    dynamic range = null;
                    try
                    {
                        range = worksheet.Range[namedRange.Key];
                    }
                    catch (COMException ex)
                    {
                        // Handle the case where the named range is not found
                        //Console.WriteLine($"Named range {namedRange.Key} not found: {ex.Message}");
                    }

                    // If the named range is found, set its value
                    if (range != null)
                    {
                        range.Value = range.Value?.ToString() + namedRange.Value;
                        Marshal.ReleaseComObject(range);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions or log errors
                    Console.WriteLine($"Erreur en voulant modifier la cellule {namedRange.Key} avec la valeur {namedRange.Value} : {ex.Message}");
                }
            }
        }

        public static void PasteDataTableToExcel(DataTable dt, string templateFilePath, string templateSheetName, string saveFilePath, Dictionary<string, object> namedRanges = null, dynamic OpenExcelApp = null, dynamic OpenWorkbook = null, List<string> extensionList = null)
        {
            //Extensions to output
            if (extensionList == null)
            {
                extensionList = new List<string> { "xlsx" };
            }

            // Use late binding to dynamically load Excel
            dynamic excelApp = null;
            if (OpenExcelApp == null)
            {
                excelApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
            }
            else
            {
                excelApp = OpenExcelApp;
            }
            excelApp.DisplayAlerts = false;
            dynamic workbook = null;
            if (OpenWorkbook == null)
            {
                // Get the template file to use
                if (templateFilePath.StartsWith("Mint."))
                {
                    string tempPath = System.IO.Path.GetTempFileName();
                    System.IO.File.WriteAllBytes(tempPath, Properties.Resources.Template_DA);
                    workbook = excelApp.Workbooks.Open(tempPath);
                }
                else
                {
                    workbook = excelApp.Workbooks.Open(templateFilePath);
                    //workbook = excelApp.Workbooks.Open(FileName: templateFilePath);
                }
            }
            else
            {
                workbook = excelApp.Workbooks.Add();
                OpenWorkbook.Sheets.Copy(After: workbook.Sheets[1]);
                workbook.Sheets[1].Delete();


            }

            try
            {
                // Get the worksheet by name
                //Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[templateSheetName];
                dynamic worksheet = workbook.Sheets[templateSheetName];

                // Find the named range "R_START" and get its range
                dynamic startRange = worksheet.Range["R_TABLE"];

                // Calculate the number of rows needed based on the DataTable
                int numRows = dt.Rows.Count;
                int numColumns = dt.Columns.Count;

                // Insert rows below "R_START"
                if (numRows > 1) { startRange.Offset[1, 0].Resize[numRows - 1, 1].EntireRow.Insert(); }

                // Get the resized range (including the inserted rows)
                dynamic resizedRange = startRange.Resize[numRows, numColumns];
                // Copy the DataTable data to the Excel range cell by cell
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numColumns; j++)
                    {
                        resizedRange.Cells[i + 1, j + 1].Value = dt.Rows[i][j];
                    }
                }

                // Complete other named ranges
                if (namedRanges != null) { CompleteNamedRanges(worksheet, namedRanges); }

                // Save the changes
                int countXL = 0;
                foreach (string extension in extensionList)
                {
                    if (extension != "pdf")
                    {   
                        workbook.SaveAs(saveFilePath.Replace(".xlsx", "." + extension));
                        countXL++;
                    }
                }
                if (extensionList.Contains("pdf"))
                {
                    workbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, saveFilePath.Replace(".xlsx", ".pdf"));
                }
                else if (!extensionList.Contains("pdf") && countXL > 0)
                {
                    workbook.Close();
                }

                // Optional: Open the Excel application for viewing
                //excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("Erreur lors de la création du fichier Excel : " + ex.Message,"Erreur");
            }
            finally
            {
                // Close and release Excel objects

                Marshal.ReleaseComObject(workbook);
                if (OpenExcelApp == null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        public static void OpenInExcel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("Ca ne marche pas avec cette table");
            }

            // Use late binding to dynamically load Excel
            dynamic excelApp = null;
            try
            {
                excelApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                excelApp.Visible = true;
                excelApp.DisplayAlerts = false;

                dynamic workbook = excelApp.Workbooks.Add();
                dynamic worksheet = workbook.Sheets[1];

                // Add column headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                }

                // Add rows
                object[,] data = new object[dt.Rows.Count, dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        data[i, j] = dt.Rows[i][j];
                    }
                }
                worksheet.Range["A2"].Resize[dt.Rows.Count, dt.Columns.Count].Value = data;

                // Auto-fit columns
                worksheet.Columns.AutoFit();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Ca ne marche pas avec cette table: {ex.Message}");
            }
            finally
            {
                if (excelApp != null)
                {
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }
    }
}
