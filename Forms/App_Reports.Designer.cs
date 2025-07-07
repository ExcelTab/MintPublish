namespace Mint.Forms
{
    partial class App_Reports
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App_Reports));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            panel1 = new Panel();
            checkBox_pdf = new CheckBox();
            checkBox_xlsx = new CheckBox();
            label6 = new Label();
            checkBox_csv = new CheckBox();
            label5 = new Label();
            comboBox_Artist = new ComboBox();
            pictureBox_GoArtist = new PictureBox();
            pictureBox_PathArtist = new PictureBox();
            textBox_PathArtist = new TextBox();
            label4 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            dateTimePicker_To = new DateTimePicker();
            dateTimePicker_From = new DateTimePicker();
            button1 = new Button();
            tabControl_Reports = new TabControl();
            tabPage_Visual = new TabPage();
            chart_Visual = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panel2 = new Panel();
            panel_VisualDates = new Panel();
            dateTimePicker_VisualTo = new DateTimePicker();
            label9 = new Label();
            dateTimePicker_VisualFrom = new DateTimePicker();
            label10 = new Label();
            label8 = new Label();
            label7 = new Label();
            comboBox_VisualPeriod = new ComboBox();
            comboBox_VisualSource = new ComboBox();
            tabPage_Reports = new TabPage();
            panel3 = new Panel();
            checkBox_Miroir = new CheckBox();
            textBox_textePic = new TextBox();
            label12 = new Label();
            textBox_margePic = new TextBox();
            label11 = new Label();
            textBox_nombrePic = new TextBox();
            textBox_URLPic = new TextBox();
            pictureBox_GoPic = new PictureBox();
            pictureBox_PathPic = new PictureBox();
            textBox_pathPic = new TextBox();
            label13 = new Label();
            label14 = new Label();
            pictureBox4 = new PictureBox();
            label15 = new Label();
            label16 = new Label();
            button_Pic = new Button();
            imageList1 = new ImageList(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_GoArtist).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PathArtist).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl_Reports.SuspendLayout();
            tabPage_Visual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chart_Visual).BeginInit();
            panel2.SuspendLayout();
            panel_VisualDates.SuspendLayout();
            tabPage_Reports.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_GoPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PathPic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBox_pdf);
            panel1.Controls.Add(checkBox_xlsx);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(checkBox_csv);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(comboBox_Artist);
            panel1.Controls.Add(pictureBox_GoArtist);
            panel1.Controls.Add(pictureBox_PathArtist);
            panel1.Controls.Add(textBox_PathArtist);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(dateTimePicker_To);
            panel1.Controls.Add(dateTimePicker_From);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(8, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(246, 372);
            panel1.TabIndex = 0;
            // 
            // checkBox_pdf
            // 
            checkBox_pdf.AutoSize = true;
            checkBox_pdf.Checked = true;
            checkBox_pdf.CheckState = CheckState.Checked;
            checkBox_pdf.Location = new Point(175, 270);
            checkBox_pdf.Name = "checkBox_pdf";
            checkBox_pdf.Size = new Size(47, 19);
            checkBox_pdf.TabIndex = 19;
            checkBox_pdf.Text = ".pdf";
            checkBox_pdf.UseVisualStyleBackColor = true;
            // 
            // checkBox_xlsx
            // 
            checkBox_xlsx.AutoSize = true;
            checkBox_xlsx.Checked = true;
            checkBox_xlsx.CheckState = CheckState.Checked;
            checkBox_xlsx.Location = new Point(120, 270);
            checkBox_xlsx.Name = "checkBox_xlsx";
            checkBox_xlsx.Size = new Size(49, 19);
            checkBox_xlsx.TabIndex = 18;
            checkBox_xlsx.Text = ".xlsx";
            checkBox_xlsx.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 271);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 17;
            label6.Text = "format :";
            // 
            // checkBox_csv
            // 
            checkBox_csv.AutoSize = true;
            checkBox_csv.Location = new Point(65, 270);
            checkBox_csv.Name = "checkBox_csv";
            checkBox_csv.Size = new Size(46, 19);
            checkBox_csv.TabIndex = 16;
            checkBox_csv.Text = ".csv";
            checkBox_csv.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 242);
            label5.Name = "label5";
            label5.Size = new Size(45, 15);
            label5.TabIndex = 15;
            label5.Text = "artiste :";
            // 
            // comboBox_Artist
            // 
            comboBox_Artist.FormattingEnabled = true;
            comboBox_Artist.Location = new Point(62, 239);
            comboBox_Artist.Name = "comboBox_Artist";
            comboBox_Artist.Size = new Size(181, 23);
            comboBox_Artist.TabIndex = 14;
            // 
            // pictureBox_GoArtist
            // 
            pictureBox_GoArtist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_GoArtist.Image = Properties.Resources.location;
            pictureBox_GoArtist.Location = new Point(217, 315);
            pictureBox_GoArtist.Name = "pictureBox_GoArtist";
            pictureBox_GoArtist.Size = new Size(26, 23);
            pictureBox_GoArtist.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_GoArtist.TabIndex = 13;
            pictureBox_GoArtist.TabStop = false;
            pictureBox_GoArtist.Click += pictureBox_GoArtist_Click;
            // 
            // pictureBox_PathArtist
            // 
            pictureBox_PathArtist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_PathArtist.Image = Properties.Resources.folder;
            pictureBox_PathArtist.Location = new Point(185, 315);
            pictureBox_PathArtist.Name = "pictureBox_PathArtist";
            pictureBox_PathArtist.Size = new Size(26, 23);
            pictureBox_PathArtist.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_PathArtist.TabIndex = 12;
            pictureBox_PathArtist.TabStop = false;
            pictureBox_PathArtist.Click += pictureBox_PathArtist_Click;
            // 
            // textBox_PathArtist
            // 
            textBox_PathArtist.BackColor = Color.FromArgb(255, 255, 192);
            textBox_PathArtist.Location = new Point(13, 315);
            textBox_PathArtist.Name = "textBox_PathArtist";
            textBox_PathArtist.Size = new Size(166, 23);
            textBox_PathArtist.TabIndex = 8;
            textBox_PathArtist.TextAlign = HorizontalAlignment.Right;
            textBox_PathArtist.TextChanged += textBox_PathArtist_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 291);
            label4.Name = "label4";
            label4.Size = new Size(130, 15);
            label4.TabIndex = 7;
            label4.Text = "Dossier de sauvegarde :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(65, 3);
            label3.Name = "label3";
            label3.Size = new Size(119, 21);
            label3.TabIndex = 6;
            label3.Text = "Droits d'auteur";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(11, 24);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(232, 151);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 216);
            label2.Name = "label2";
            label2.Size = new Size(26, 15);
            label2.TabIndex = 4;
            label2.Text = "au :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 187);
            label1.Name = "label1";
            label1.Size = new Size(27, 15);
            label1.TabIndex = 3;
            label1.Text = "du :";
            // 
            // dateTimePicker_To
            // 
            dateTimePicker_To.Location = new Point(43, 210);
            dateTimePicker_To.Name = "dateTimePicker_To";
            dateTimePicker_To.Size = new Size(200, 23);
            dateTimePicker_To.TabIndex = 2;
            // 
            // dateTimePicker_From
            // 
            dateTimePicker_From.Location = new Point(43, 181);
            dateTimePicker_From.Name = "dateTimePicker_From";
            dateTimePicker_From.Size = new Size(200, 23);
            dateTimePicker_From.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(61, 346);
            button1.Name = "button1";
            button1.Size = new Size(111, 23);
            button1.TabIndex = 0;
            button1.Text = "Sortir le rapport";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabControl_Reports
            // 
            tabControl_Reports.Controls.Add(tabPage_Visual);
            tabControl_Reports.Controls.Add(tabPage_Reports);
            tabControl_Reports.Dock = DockStyle.Fill;
            tabControl_Reports.ImageList = imageList1;
            tabControl_Reports.Location = new Point(0, 0);
            tabControl_Reports.Name = "tabControl_Reports";
            tabControl_Reports.SelectedIndex = 0;
            tabControl_Reports.Size = new Size(800, 450);
            tabControl_Reports.TabIndex = 1;
            // 
            // tabPage_Visual
            // 
            tabPage_Visual.Controls.Add(chart_Visual);
            tabPage_Visual.Controls.Add(panel2);
            tabPage_Visual.ImageIndex = 0;
            tabPage_Visual.Location = new Point(4, 24);
            tabPage_Visual.Name = "tabPage_Visual";
            tabPage_Visual.Padding = new Padding(3);
            tabPage_Visual.Size = new Size(792, 422);
            tabPage_Visual.TabIndex = 0;
            tabPage_Visual.Text = "Rapport visuel";
            tabPage_Visual.UseVisualStyleBackColor = true;
            // 
            // chart_Visual
            // 
            chart_Visual.BackgroundImageLayout = ImageLayout.None;
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IntervalOffset = 1D;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = -90;
            chartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.IsMarksNextToAxis = false;
            chartArea1.AxisY.LineColor = Color.Transparent;
            chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisY2.LabelStyle.Format = "0.00 €";
            chartArea1.AxisY2.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.BorderColor = Color.Transparent;
            chartArea1.Name = "ChartArea1";
            chart_Visual.ChartAreas.Add(chartArea1);
            chart_Visual.Dock = DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            chart_Visual.Legends.Add(legend1);
            chart_Visual.Location = new Point(3, 37);
            chart_Visual.Name = "chart_Visual";
            chart_Visual.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.BorderColor = Color.Gray;
            series1.ChartArea = "ChartArea1";
            series1.Color = Color.FromArgb(255, 192, 128);
            series1.IsValueShownAsLabel = true;
            series1.LabelBackColor = Color.DimGray;
            series1.LabelBorderColor = Color.Black;
            series1.LabelForeColor = Color.FromArgb(255, 192, 128);
            series1.Legend = "Legend1";
            series1.LegendText = "CA (Tot HT)";
            series1.Name = "Series2";
            series1.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.BorderColor = Color.DimGray;
            series2.BorderWidth = 4;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = Color.Blue;
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.LegendText = "Nb commandes";
            series2.Name = "Series1";
            chart_Visual.Series.Add(series1);
            chart_Visual.Series.Add(series2);
            chart_Visual.Size = new Size(786, 382);
            chart_Visual.TabIndex = 2;
            chart_Visual.Text = "chart1";
            title1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            title1.Name = "Title1";
            title1.Text = "Evolution nb commandes et CA (ht)";
            chart_Visual.Titles.Add(title1);
            // 
            // panel2
            // 
            panel2.Controls.Add(panel_VisualDates);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(comboBox_VisualPeriod);
            panel2.Controls.Add(comboBox_VisualSource);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(786, 34);
            panel2.TabIndex = 0;
            // 
            // panel_VisualDates
            // 
            panel_VisualDates.Controls.Add(dateTimePicker_VisualTo);
            panel_VisualDates.Controls.Add(label9);
            panel_VisualDates.Controls.Add(dateTimePicker_VisualFrom);
            panel_VisualDates.Controls.Add(label10);
            panel_VisualDates.Location = new Point(425, 3);
            panel_VisualDates.Name = "panel_VisualDates";
            panel_VisualDates.Size = new Size(269, 26);
            panel_VisualDates.TabIndex = 1;
            panel_VisualDates.Visible = false;
            // 
            // dateTimePicker_VisualTo
            // 
            dateTimePicker_VisualTo.CustomFormat = "dd-MM-yyyy";
            dateTimePicker_VisualTo.Dock = DockStyle.Left;
            dateTimePicker_VisualTo.Format = DateTimePickerFormat.Custom;
            dateTimePicker_VisualTo.Location = new Point(159, 0);
            dateTimePicker_VisualTo.Name = "dateTimePicker_VisualTo";
            dateTimePicker_VisualTo.Size = new Size(106, 23);
            dateTimePicker_VisualTo.TabIndex = 5;
            dateTimePicker_VisualTo.Value = new DateTime(2024, 3, 10, 0, 0, 0, 0);
            dateTimePicker_VisualTo.ValueChanged += dateTimePicker_VisualTo_ValueChanged;
            // 
            // label9
            // 
            label9.Dock = DockStyle.Left;
            label9.Location = new Point(133, 0);
            label9.Name = "label9";
            label9.Size = new Size(26, 26);
            label9.TabIndex = 7;
            label9.Text = "au :";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dateTimePicker_VisualFrom
            // 
            dateTimePicker_VisualFrom.CustomFormat = "dd-MM-yyyy";
            dateTimePicker_VisualFrom.Dock = DockStyle.Left;
            dateTimePicker_VisualFrom.Format = DateTimePickerFormat.Custom;
            dateTimePicker_VisualFrom.Location = new Point(27, 0);
            dateTimePicker_VisualFrom.Name = "dateTimePicker_VisualFrom";
            dateTimePicker_VisualFrom.Size = new Size(106, 23);
            dateTimePicker_VisualFrom.TabIndex = 4;
            dateTimePicker_VisualFrom.Value = new DateTime(2024, 3, 3, 0, 0, 0, 0);
            dateTimePicker_VisualFrom.ValueChanged += dateTimePicker_VisualFrom_ValueChanged;
            // 
            // label10
            // 
            label10.Dock = DockStyle.Left;
            label10.Location = new Point(0, 0);
            label10.Name = "label10";
            label10.Size = new Size(27, 26);
            label10.TabIndex = 8;
            label10.Text = "du :";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(252, 6);
            label8.Name = "label8";
            label8.Size = new Size(53, 15);
            label8.TabIndex = 3;
            label8.Text = "Période :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 6);
            label7.Name = "label7";
            label7.Size = new Size(49, 15);
            label7.TabIndex = 2;
            label7.Text = "Source :";
            // 
            // comboBox_VisualPeriod
            // 
            comboBox_VisualPeriod.FormattingEnabled = true;
            comboBox_VisualPeriod.Items.AddRange(new object[] { "7 jours", "15 jours", "30 jours", "personnalisée" });
            comboBox_VisualPeriod.Location = new Point(311, 3);
            comboBox_VisualPeriod.Name = "comboBox_VisualPeriod";
            comboBox_VisualPeriod.Size = new Size(108, 23);
            comboBox_VisualPeriod.TabIndex = 1;
            comboBox_VisualPeriod.Text = "7 jours";
            comboBox_VisualPeriod.SelectedIndexChanged += comboBox_VisualPeriod_SelectedIndexChanged;
            // 
            // comboBox_VisualSource
            // 
            comboBox_VisualSource.FormattingEnabled = true;
            comboBox_VisualSource.Location = new Point(58, 3);
            comboBox_VisualSource.Name = "comboBox_VisualSource";
            comboBox_VisualSource.Size = new Size(179, 23);
            comboBox_VisualSource.TabIndex = 0;
            comboBox_VisualSource.SelectedIndexChanged += comboBox_VisualSource_SelectedIndexChanged;
            // 
            // tabPage_Reports
            // 
            tabPage_Reports.Controls.Add(panel3);
            tabPage_Reports.Controls.Add(panel1);
            tabPage_Reports.ImageIndex = 1;
            tabPage_Reports.Location = new Point(4, 24);
            tabPage_Reports.Name = "tabPage_Reports";
            tabPage_Reports.Padding = new Padding(3);
            tabPage_Reports.Size = new Size(792, 422);
            tabPage_Reports.TabIndex = 1;
            tabPage_Reports.Text = "Rapports spécifiques";
            tabPage_Reports.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(checkBox_Miroir);
            panel3.Controls.Add(textBox_textePic);
            panel3.Controls.Add(label12);
            panel3.Controls.Add(textBox_margePic);
            panel3.Controls.Add(label11);
            panel3.Controls.Add(textBox_nombrePic);
            panel3.Controls.Add(textBox_URLPic);
            panel3.Controls.Add(pictureBox_GoPic);
            panel3.Controls.Add(pictureBox_PathPic);
            panel3.Controls.Add(textBox_pathPic);
            panel3.Controls.Add(label13);
            panel3.Controls.Add(label14);
            panel3.Controls.Add(pictureBox4);
            panel3.Controls.Add(label15);
            panel3.Controls.Add(label16);
            panel3.Controls.Add(button_Pic);
            panel3.Location = new Point(260, 6);
            panel3.Name = "panel3";
            panel3.Size = new Size(254, 372);
            panel3.TabIndex = 1;
            // 
            // checkBox_Miroir
            // 
            checkBox_Miroir.AutoSize = true;
            checkBox_Miroir.Location = new Point(147, 215);
            checkBox_Miroir.Name = "checkBox_Miroir";
            checkBox_Miroir.Size = new Size(58, 19);
            checkBox_Miroir.TabIndex = 26;
            checkBox_Miroir.Text = "Miroir";
            checkBox_Miroir.UseVisualStyleBackColor = true;
            // 
            // textBox_textePic
            // 
            textBox_textePic.BackColor = Color.FromArgb(255, 255, 192);
            textBox_textePic.Location = new Point(105, 239);
            textBox_textePic.Name = "textBox_textePic";
            textBox_textePic.Size = new Size(138, 23);
            textBox_textePic.TabIndex = 25;
            textBox_textePic.Text = "WIA-73925";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(10, 242);
            label12.Name = "label12";
            label12.Size = new Size(89, 15);
            label12.TabIndex = 24;
            label12.Text = "Texte a ajouter :";
            // 
            // textBox_margePic
            // 
            textBox_margePic.BackColor = Color.FromArgb(255, 255, 192);
            textBox_margePic.Location = new Point(99, 213);
            textBox_margePic.Name = "textBox_margePic";
            textBox_margePic.Size = new Size(42, 23);
            textBox_margePic.TabIndex = 23;
            textBox_margePic.Text = "300";
            textBox_margePic.TextAlign = HorizontalAlignment.Right;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(10, 216);
            label11.Name = "label11";
            label11.Size = new Size(96, 15);
            label11.TabIndex = 22;
            label11.Text = "Marge en pixels :";
            // 
            // textBox_nombrePic
            // 
            textBox_nombrePic.BackColor = Color.FromArgb(255, 255, 192);
            textBox_nombrePic.Location = new Point(120, 268);
            textBox_nombrePic.Name = "textBox_nombrePic";
            textBox_nombrePic.Size = new Size(123, 23);
            textBox_nombrePic.TabIndex = 21;
            textBox_nombrePic.Text = "5";
            textBox_nombrePic.TextAlign = HorizontalAlignment.Right;
            // 
            // textBox_URLPic
            // 
            textBox_URLPic.BackColor = Color.FromArgb(255, 255, 192);
            textBox_URLPic.Location = new Point(54, 184);
            textBox_URLPic.Name = "textBox_URLPic";
            textBox_URLPic.Size = new Size(189, 23);
            textBox_URLPic.TabIndex = 20;
            textBox_URLPic.Text = "https://hexoa.fr/visuels/WIA73925.jpg";
            textBox_URLPic.TextAlign = HorizontalAlignment.Right;
            // 
            // pictureBox_GoPic
            // 
            pictureBox_GoPic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_GoPic.Image = Properties.Resources.location;
            pictureBox_GoPic.Location = new Point(217, 315);
            pictureBox_GoPic.Name = "pictureBox_GoPic";
            pictureBox_GoPic.Size = new Size(26, 23);
            pictureBox_GoPic.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_GoPic.TabIndex = 13;
            pictureBox_GoPic.TabStop = false;
            // 
            // pictureBox_PathPic
            // 
            pictureBox_PathPic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox_PathPic.Image = Properties.Resources.folder;
            pictureBox_PathPic.Location = new Point(185, 315);
            pictureBox_PathPic.Name = "pictureBox_PathPic";
            pictureBox_PathPic.Size = new Size(26, 23);
            pictureBox_PathPic.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_PathPic.TabIndex = 12;
            pictureBox_PathPic.TabStop = false;
            pictureBox_PathPic.Click += pictureBox_PathPic_Click;
            // 
            // textBox_pathPic
            // 
            textBox_pathPic.BackColor = Color.FromArgb(255, 255, 192);
            textBox_pathPic.Location = new Point(13, 315);
            textBox_pathPic.Name = "textBox_pathPic";
            textBox_pathPic.Size = new Size(166, 23);
            textBox_pathPic.TabIndex = 8;
            textBox_pathPic.Text = "C:/Temp/";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(11, 291);
            label13.Name = "label13";
            label13.Size = new Size(130, 15);
            label13.TabIndex = 7;
            label13.Text = "Dossier de sauvegarde :";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label14.Location = new Point(23, 3);
            label14.Name = "label14";
            label14.Size = new Size(208, 21);
            label14.TabIndex = 6;
            label14.Text = "Outil Script Image (en dev.)";
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(61, 24);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(147, 151);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 5;
            pictureBox4.TabStop = false;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(11, 271);
            label15.Name = "label15";
            label15.Size = new Size(95, 15);
            label15.TabIndex = 4;
            label15.Text = "Nombre de fois :";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(10, 187);
            label16.Name = "label16";
            label16.Size = new Size(34, 15);
            label16.TabIndex = 3;
            label16.Text = "URL :";
            // 
            // button_Pic
            // 
            button_Pic.Location = new Point(61, 346);
            button_Pic.Name = "button_Pic";
            button_Pic.Size = new Size(111, 23);
            button_Pic.TabIndex = 0;
            button_Pic.Text = "Tester l'outil";
            button_Pic.UseVisualStyleBackColor = true;
            button_Pic.Click += button_Pic_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "148786.png");
            imageList1.Images.SetKeyName(1, "148840.png");
            // 
            // App_Reports
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl_Reports);
            Name = "App_Reports";
            Text = "App_Reports";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_GoArtist).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PathArtist).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl_Reports.ResumeLayout(false);
            tabPage_Visual.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chart_Visual).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel_VisualDates.ResumeLayout(false);
            tabPage_Reports.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_GoPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PathPic).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private DateTimePicker dateTimePicker_To;
        private DateTimePicker dateTimePicker_From;
        private Button button1;
        private TextBox textBox_PathArtist;
        private Label label4;
        private PictureBox pictureBox_GoArtist;
        private PictureBox pictureBox_PathArtist;
        private CheckBox checkBox_pdf;
        private CheckBox checkBox_xlsx;
        private Label label6;
        private CheckBox checkBox_csv;
        private Label label5;
        private ComboBox comboBox_Artist;
        private TabControl tabControl_Reports;
        private TabPage tabPage_Visual;
        private Panel panel_VisualDates;
        private Panel panel2;
        private Label label10;
        private Label label9;
        private DateTimePicker dateTimePicker_VisualFrom;
        private DateTimePicker dateTimePicker_VisualTo;
        private Label label8;
        private Label label7;
        private ComboBox comboBox_VisualPeriod;
        private ComboBox comboBox_VisualSource;
        private TabPage tabPage_Reports;
        private ImageList imageList1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Visual;
        private Panel panel3;
        private CheckBox checkBox_Miroir;
        private CheckBox checkBox2;
        private Label label11;
        private CheckBox checkBox3;
        private Label label12;
        private ComboBox comboBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private TextBox textBox1;
        private Label label13;
        private Label label14;
        private PictureBox pictureBox4;
        private Label label15;
        private Label label16;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Button button_Pic;
        private TextBox textBox_nombrePic;
        private TextBox textBox_URLPic;
        private TextBox textBox_textePic;
        private TextBox textBox_margePic;
       private PictureBox pictureBox_GoPic;
        private PictureBox pictureBox_PathPic;
        private TextBox textBox_pathPic;

    }
}