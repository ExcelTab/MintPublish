using iText.Kernel.Geom;
using System.Data;
using System.IO;
using System.Text;

namespace Mint.Code
{
    internal class CSV
    {
        public static void CreateCsv(DataTable dt, string filePath, bool open = false)
        {
            //Check if filepath is already opened. If so, reename filePath with a random number
            while (true)
            {
                try
                {
                    using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        // File is not in use, break the loop
                        break;
                    }
                }
                catch (IOException)
                {
                    // File is in use, generate a new file path with a random number
                    filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePath), System.IO.Path.GetFileNameWithoutExtension(filePath) + "_" + new Random().Next(1000, 9999).ToString() + System.IO.Path.GetExtension(filePath));
                }
            }



            // Create a new StreamWriter and specify the file path
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write the column headers
                foreach (DataColumn column in dt.Columns)
                {
                    writer.Write(column.ColumnName);
                    writer.Write(";");
                }
                writer.WriteLine();

                // Write the data rows
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        writer.Write(row[i].ToString());
                        writer.Write(";");
                    }
                    writer.WriteLine();
                }
            }
            if (open == true) { System.Diagnostics.Process.Start("explorer.exe", filePath); }
        }
    }
}
