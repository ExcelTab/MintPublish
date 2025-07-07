using Mint.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Drawing.Imaging;

namespace Mint.Forms
{
    public partial class App_Reports : Form
    {
        #region Global Events
        public delegate void StatusUpdateDelegate(string message);
        public static event StatusUpdateDelegate StatusUpdate;
        #endregion

        #region@Global Variables

        #endregion

        public App_Reports()
        {
            InitializeComponent();
            LoadSettings();


            //Load Visual Sources
            comboBox_VisualSource.Items.Add("TOUTES");
            comboBox_VisualSource.Items.Add("");

            //Set the dates of VisualFrom and VisualTo to the first and last day of the current month
            //Prevent the evetns of those datetimepickers to be triggered
            dateTimePicker_VisualFrom.ValueChanged -= dateTimePicker_VisualFrom_ValueChanged;
            dateTimePicker_VisualTo.ValueChanged -= dateTimePicker_VisualTo_ValueChanged;
            dateTimePicker_VisualFrom.Value = DateTime.Today.AddDays(-8);
            dateTimePicker_VisualTo.Value = DateTime.Today.AddDays(-1);
            dateTimePicker_VisualFrom.ValueChanged += dateTimePicker_VisualFrom_ValueChanged;
            dateTimePicker_VisualTo.ValueChanged += dateTimePicker_VisualTo_ValueChanged;

            string query = "SELECT distinct source_order From TBL_Order order by source_order asc";
            DataTable dt = Database.LoadData(query);
            foreach (DataRow row in dt.Rows)
            {
                comboBox_VisualSource.Items.Add(row["source_order"].ToString());
            }
            //Ceci va lancer le refresh du rapport visuel donc pas besoin de l'appeler ici
            comboBox_VisualSource.Text = "TOUTES";

            //Load Combobox Artist
            comboBox_Artist.Items.Add("TOUS");
            comboBox_Artist.Items.Add("");

            //Deactivate XLS possibilities if Excel is not installed
            bool XLInstalled = XLSX.IsExcelInstalled();
            if (!XLInstalled)
            {
                checkBox_pdf.Checked = false;
                checkBox_pdf.Enabled = false;
                checkBox_xlsx.Checked = false;
                checkBox_xlsx.Enabled = false;
                checkBox_csv.Checked = true;
            }


            query = "SELECT id_manufacturer, name From TBL_MANUFACTURER order by name asc";
            dt = Database.LoadData(query);
            foreach (DataRow row in dt.Rows)
            {
                comboBox_Artist.Items.Add(row["name"].ToString());
            }
            comboBox_Artist.Text = "TOUS";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedArtist = comboBox_Artist.Text;
            if (selectedArtist == "TOUS" || selectedArtist == "") { selectedArtist = null; }
            await Task.Run(async () => { await GetArtistReports(selectedArtist); });
        }
        private async Task GetArtistReports(string selectedArtist = null)
        {
            StatusUpdate?.Invoke($"Sortie des rapports de Droits d'Auteurs...");

            string fromDate = dateTimePicker_From.Value.ToString("yyyy-MM-dd");
            string toDate = dateTimePicker_To.Value.ToString("yyyy-MM-dd");
            string saveFolderPath = textBox_PathArtist.Text;
            if (saveFolderPath == "")
            {
                MessageBox.Show("Veuillez d'abord choisir un dossier de sauvegarde");
                StatusUpdate?.Invoke("");
                return;
            }


            string viewQuery = "Report_Droit_Auteur";
            string viewQuerySQL = Database.GetViewDefinition(viewQuery);
            viewQuerySQL = viewQuerySQL.Replace("ORDER BY", "AND date_order between '" + fromDate + "' and '" + toDate + "' ORDER BY");
            DataTable dtReport = Database.LoadData(viewQuerySQL);

            //Get the artist data
            string query = "SELECT * from TBL_MANUFACTURER M LEFT JOIN TBL_MANUFACTURER_EXTENDED ME on M.id_manufacturer = ME.id_manufacturer where is_active = 1";
            DataTable dtArtist = null;
            if (selectedArtist != null)
            {
                query += " and M.name = @artistName";
                //send the artist name as parameter
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@artistName", selectedArtist);
                dtArtist = Database.LoadData(query, false, parameters);
            }
            else
            {
                dtArtist = Database.LoadData(query);
            }

            if (dtArtist.Rows.Count == 0)
            {
                MessageBox.Show("Aucune vente ne correspond à vos critères.");
                StatusUpdate?.Invoke("");
                return;
            }

            int count = 0;

            //If output in Excel, load the Excel Application
            dynamic excelApp = null;
            dynamic workbook = null;
            bool XLInstalled = XLSX.IsExcelInstalled();
            List<string> extensionList = new List<string>();
            if (checkBox_xlsx.Checked || checkBox_pdf.Checked)
            {
                excelApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                string tempPath = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllBytes(tempPath, Properties.Resources.Template_DA);
                workbook = excelApp.Workbooks.Open(tempPath);
                if (checkBox_xlsx.Checked) { extensionList.Add("xlsx"); }
                if (checkBox_pdf.Checked) { extensionList.Add("pdf"); }

            }

            //Loop through all artist and create a separate datatable
            foreach (DataRow row in dtArtist.Rows)
            {
                count++;
                StatusUpdate?.Invoke($"Sortie des rapports de Droits d'Auteurs... {count}/{dtArtist.Rows.Count} - {row["name"].ToString()}");
                DataRow[] ArtistRows = dtReport.Select("id_manufacturer = " + row["id_manufacturer"].ToString());
                if (ArtistRows.Length == 0 && selectedArtist != null)
                {
                    MessageBox.Show("Aucune vente ne correspond à vos critères.");
                    StatusUpdate?.Invoke("");
                    return;
                }
                else if (ArtistRows.Length > 0)
                {
                    //Create a new datatable
                    DataTable dtArtistReport = dtReport.Clone();
                    foreach (DataRow rowArtistReport in ArtistRows)
                    {
                        dtArtistReport.ImportRow(rowArtistReport);
                    }
                    dtArtistReport.Columns.Remove("id_manufacturer");

                    //Create the report
                    string reportname;
                    if (row["code3"].ToString() == "")
                    {
                        reportname = row["name"].ToString().ToString().Replace("'", "");
                    }
                    else
                    {
                        reportname = row["code3"].ToString();
                    }
                    reportname = reportname += "_" + fromDate + "_" + toDate;


                    string templateFilePath = "Mint.Resources.Template DA.xlsx";
                    string templateSheetName = "Total";
                    string saveFilePath = "";
                    Dictionary<string, object> namedRanges = new Dictionary<string, object>();
                    namedRanges.Add("R_HEADER", row["name"].ToString());
                    namedRanges.Add("R_ADRESS", row["address"].ToString());
                    namedRanges.Add("R_EMAIL", row["email"].ToString());
                    namedRanges.Add("R_ENTRY", row["date_entry"].ToString());
                    namedRanges.Add("R_PAYMENT", row["paiement_method"].ToString());
                    namedRanges.Add("R_REPORTNAME", reportname);
                    namedRanges.Add("R_TIME", "du " + dateTimePicker_From.Value.ToString("dd MMMM yyyy") + " au " + dateTimePicker_To.Value.ToString("dd MMMM yyyy"));
                    // Convert TVA and COMMISSION to percentage strings
                    string tvaPercentage = (Convert.ToDouble(row["TVA"]) * 100).ToString() + "%";
                    string commissionPercentage = (Convert.ToDouble(row["commission"]) * 100).ToString() + "%";

                    namedRanges.Add("R_TVA", tvaPercentage);
                    namedRanges.Add("R_COMMISSION", commissionPercentage);


                    if (checkBox_csv.Checked)
                    {
                        saveFilePath = saveFolderPath + @"\Droit_Auteur_" + reportname + ".csv";
                        CSV.CreateCsv(dtArtistReport, saveFilePath, false);
                    }
                    if (checkBox_xlsx.Checked || checkBox_pdf.Checked)
                    {
                        saveFilePath = saveFolderPath + @"\Droit_Auteur_" + reportname + ".xlsx";
                        try
                        {
                            // Code causing the issue
                            XLSX.PasteDataTableToExcel(dtArtistReport, templateFilePath, templateSheetName, saveFilePath, namedRanges, excelApp, workbook, extensionList);
                        }
                        catch (Exception ex)
                        {
                            // Log the outer exception
                            MessageBox.Show($"Outer Exception: {ex.Message}");

                            // Log the inner exception if present
                            if (ex.InnerException != null)
                            {
                                MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
                            }
                        }
                    }
                }
            }

            //Close the Excel Application
            if (checkBox_xlsx.Checked || checkBox_pdf.Checked)
            {
                workbook.Close(false);
                Marshal.ReleaseComObject(workbook);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }

            StatusUpdate?.Invoke("");
            System.Diagnostics.Process.Start("explorer.exe", saveFolderPath);
        }

        private void LoadSettings()
        {
            //Load the settings from the App resources
            textBox_PathArtist.Text = Properties.Settings.Default.artistPath;


        }

        private void pictureBox_PathArtist_Click(object sender, EventArgs e)
        {
            textBox_PathArtist.Text = Code.BasicFunctions.ChoosePath("Choisissez le dossier pour la sauvegarde des rapports de droits d'auteur", "Folder");
        }

        private void pictureBox_GoArtist_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox_PathArtist.Text);
        }
        private void textBox_PathArtist_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.artistPath = textBox_PathArtist.Text;
            Properties.Settings.Default.Save();
        }

        private async Task RefreshVisualReport()
        {
            if (tabControl_Reports.SelectedTab != tabPage_Visual) { return; }
            this.Cursor = Cursors.WaitCursor;

            chart_Visual.Series[0].Points.Clear();
            chart_Visual.Series[1].Points.Clear();

            string sSource = comboBox_VisualSource.Text;
            string sPeriod = comboBox_VisualPeriod.Text;
            string fromDate = dateTimePicker_VisualFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dateTimePicker_VisualTo.Value.AddDays(1).ToString("yyyy-MM-dd");

            //string query = $@"SELECT CONVERT(date, date_order) as cdate, count(id_order) as nbo, sum(total_ht) as totht
            //              from TBL_ORDER WHERE (date_order between ";
            //string query = $@"SELECT FORMAT(date_order, 'yyyy-MM-dd') as cdate, count(id_order) as nbo, sum(total_ht) as totht
            //              from TBL_ORDER WHERE (date_order between ";


            string query = $@"SELECT FORMAT(DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time', 'yyyy-MM-dd') AS cdate, 
            COUNT(id_order) AS nbo, SUM(total_ht) AS totht FROM TBL_ORDER 
            WHERE ((DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time') 
            BETWEEN ";

            switch (sPeriod)
            {
                case "7 jours":
                    fromDate = DateTime.Today.AddDays(-8).ToString("yyyy-MM-dd");
                    toDate = DateTime.Today.AddDays(0).ToString("yyyy-MM-dd");
                    break;
                case "15 jours":
                    fromDate = DateTime.Today.AddDays(-16).ToString("yyyy-MM-dd");
                    toDate = DateTime.Today.AddDays(0).ToString("yyyy-MM-dd");

                    break;
                case "30 jours":
                    fromDate = DateTime.Today.AddDays(-31).ToString("yyyy-MM-dd");
                    toDate = DateTime.Today.AddDays(0).ToString("yyyy-MM-dd");
                    break;
                case "personnalisée":
                    fromDate = fromDate;
                    toDate = toDate;
                    break;
            }
            query += $" '{fromDate}' AND '{toDate}')";

            DataTable dtVisual = null;
            if (sSource != "TOUTES")
            {
                //Modification du 19/04/2024 : retrait des exclusions id_order_state
                query += $@" and source_order = @sourceName 
                            and id_order_state not in (1,6,8,10,11,12,13,14,15,16,17,18,19,21,22,23,24,25,26,30,31,34,35)
                            GROUP BY 
                                FORMAT(DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time', 'yyyy-MM-dd') 
                            ORDER BY 
                                FORMAT(DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time', 'yyyy-MM-dd');";
                //send the artist name as parameter
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@sourceName", sSource);
                dtVisual = Database.LoadData(query, false, parameters);
            }
            else
            {
                query += $@" and id_order_state not in (1,6,8,10,11,12,13,14,15,16,17,18,19,21,22,23,24,25,26,30,31,34,35)
                            GROUP BY 
                                FORMAT(DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time', 'yyyy-MM-dd') 
                            ORDER BY 
                                FORMAT(DATE_ORDER AT TIME ZONE 'UTC' AT TIME ZONE 'Central European Standard Time', 'yyyy-MM-dd');";
                dtVisual = Database.LoadData(query);
            }

            if (dtVisual.Rows.Count > 0)
            {
                //add the from and to date to the dtVisual if they are not yet in the datatable, with 0 values
                DateTime dfromDate = DateTime.Parse(fromDate).AddDays(1);
                DateTime dtoDate = DateTime.Parse(toDate).AddSeconds(-1);
                //Set CurrentDay = year, month ,and day +1 of fromdate, at 00:00:00
                DateTime currentDay = dfromDate;



                //DateTime currentDay = DateTime.Parse(fromDate);
                while (currentDay <= dtoDate)
                {
                    if (dtVisual.Select("cdate = '" + currentDay.ToString("yyyy-MM-dd") + "'").Length == 0)
                    {
                        DataRow dr = dtVisual.NewRow();
                        dr["cdate"] = currentDay.ToString("yyyy-MM-dd");
                        dr["nbo"] = 0;
                        dr["totht"] = 0;
                        dtVisual.Rows.Add(dr);
                    }
                    currentDay = currentDay.AddDays(1);
                }

                //order the dtVisual by dates
                dtVisual.DefaultView.Sort = "cdate ASC";
                dtVisual = dtVisual.DefaultView.ToTable();

                foreach (DataRow row in dtVisual.Rows)
                {
                    string dateString = row["cdate"].ToString();
                    DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (date >= dfromDate && date <= dtoDate)
                    {
                        // Add points to the nbo series (Series[0])
                        DataPoint nboPoint = new DataPoint(date.ToOADate(), Convert.ToDouble(row["nbo"]));
                        nboPoint.AxisLabel = date.ToString("dd-MMM-yyyy");
                        chart_Visual.Series[1].Points.Add(nboPoint);

                        // Add points to the totht series (Series[1])
                        DataPoint tothtPoint = new DataPoint(date.ToOADate(), Convert.ToDouble(row["totht"]));
                        tothtPoint.AxisLabel = date.ToString("dd-MMM-yyyy");
                        chart_Visual.Series[0].Points.Add(tothtPoint);
                    }
                }

            }

            //Store the sum of ca and nbcommande in double variables
            double sumCA = 0;
            Int32 sumNbCommande = 0;
            foreach (DataRow row in dtVisual.Rows)
            {
                sumCA += Convert.ToDouble(row["totht"]);
                sumNbCommande += Convert.ToInt32(row["nbo"]);
            }
            //only keep the first 2 digits after the comma
            sumCA = Math.Round(sumCA, 2);

            //Add the totals of the series to the legend of the chart
            chart_Visual.Series[0].LegendText = $"CA (Tot HT) : {sumCA} €";
            chart_Visual.Series[1].LegendText = $"Nb commandes : {sumNbCommande}";


            this.Cursor = Cursors.Default;


        }

        //Déclencheurs d'événements pour le rapport visuel
        private void comboBox_VisualSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshVisualReport();
        }
        private void comboBox_VisualPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_VisualPeriod.Text == "personnalisée")
            {
                panel_VisualDates.Visible = true;
            }
            else
            {
                panel_VisualDates.Visible = false;
            }
            RefreshVisualReport();
        }

        private void dateTimePicker_VisualFrom_ValueChanged(object sender, EventArgs e)
        {
            //Check for incoherent dates
            if (dateTimePicker_VisualTo.Value < dateTimePicker_VisualFrom.Value)
            {
                dateTimePicker_VisualTo.Value = dateTimePicker_VisualFrom.Value;
            }
            else
            {
                RefreshVisualReport();
            }
        }
        private void dateTimePicker_VisualTo_ValueChanged(object sender, EventArgs e)
        {
            //Check for incoherent dates
            if (dateTimePicker_VisualFrom.Value > dateTimePicker_VisualTo.Value)
            {
                dateTimePicker_VisualFrom.Value = dateTimePicker_VisualTo.Value;
            }
            else
            {
                RefreshVisualReport();
            }
        }

        #region TestScriptImage
        private void pictureBox_PathPic_Click(object sender, EventArgs e)
        {
            textBox_pathPic.Text = Code.BasicFunctions.ChoosePath("Choisissez le dossier pour la sauvegarde des images modifiées", "Folder");
        }

        private void pictureBox_GoPic_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox_pathPic.Text);
        }

        private async void button_Pic_Click(object sender, EventArgs e)
        {
            string url = textBox_URLPic.Text;
            string saveFolderPath = textBox_pathPic.Text;
            int marginPx = int.Parse(textBox_margePic.Text);
            string textToAdd = textBox_textePic.Text;
            int numberOfPics = int.Parse(textBox_nombrePic.Text);
            //store the time of now in a variable
            DateTime startTime = DateTime.Now;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(saveFolderPath) || marginPx <= 0 || numberOfPics <= 0)
            {
                MessageBox.Show("Please ensure all inputs are valid.");
                return;
            }

            for (int i = 0; i < numberOfPics; i++)
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(url);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image originalImage = Image.FromStream(ms);
                        

                        int newWidth = originalImage.Width + (marginPx * 2);
                        int newHeight = originalImage.Height + (marginPx * 2);

                        using (Bitmap newImage = new Bitmap(newWidth, newHeight, originalImage.PixelFormat)) //omit pixelformat if needed
                        {
                            using (Graphics g = Graphics.FromImage(newImage))
                            {
                                g.Clear(Color.White);
                                g.DrawImage(originalImage, marginPx, marginPx, originalImage.Width, originalImage.Height);  //or use DrawImageunscaled mais ça fonctionne pas);

                                if (checkBox_Miroir.Checked)
                                {
                                    // Top mirror slice
                                    g.DrawImage(originalImage, new Rectangle(marginPx, 0, originalImage.Width, marginPx),
                                        0, marginPx, originalImage.Width, -marginPx, GraphicsUnit.Pixel);

                                    // Bottom mirror slice
                                    g.DrawImage(originalImage, new Rectangle(marginPx, newHeight - marginPx, originalImage.Width, marginPx),
                                        0, originalImage.Height - marginPx, originalImage.Width, marginPx, GraphicsUnit.Pixel);

                                    // Left mirror slice
                                    g.DrawImage(originalImage, new Rectangle(0, marginPx, marginPx, originalImage.Height),
                                        marginPx, 0, -marginPx, originalImage.Height, GraphicsUnit.Pixel);

                                    // Right mirror slice
                                    g.DrawImage(originalImage, new Rectangle(newWidth - marginPx, marginPx, marginPx, originalImage.Height),
                                        originalImage.Width - marginPx, 0, marginPx, originalImage.Height, GraphicsUnit.Pixel);
                                }


                                using (Font font = new Font("Arial", 90))
                                {
                                    g.DrawString(textToAdd, font, Brushes.Black, new PointF(marginPx + 10, newHeight - 200)); //+ marginPx + 10));
                                }
                            }

                            //string savePath = Path.Combine(saveFolderPath, $"image_{i + 1}.jpg");
                            //newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //savePath = Path.Combine(saveFolderPath, $"original_image_{i + 1}.jpg");
                            //originalImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            string savePath = Path.Combine(saveFolderPath, $"image_{i + 1}.jpg");

                            // Set JPEG quality level
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L); // 100L for highest quality
                            encoderParams.Param[0] = encoderParam;

                            newImage.Save(savePath, jpgEncoder, encoderParams);
                        }
                    }
                }
            }
            //get end time of now, then store in seconds the difference with the starttime
            double duration = (DateTime.Now - startTime).TotalSeconds;
            MessageBox.Show($"Images traitées. Ca a pris {duration} secondes");
        }

       

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

    }
}

