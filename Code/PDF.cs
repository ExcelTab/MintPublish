using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Image = iText.Layout.Element.Image;
using iText.Kernel.Pdf;
using System.Drawing.Printing;
using static iText.Layout.Borders.Border;

namespace Mint.Code
{
    internal class PDF
    {
        public static void CreatePDF(string whatPDF, string PDFPath="", string id="", string atelier="", bool directPrint = false)
        {
            //System.Reflection.Assembly assemblys = System.Reflection.Assembly.GetExecutingAssembly();
            //string[] resources = assemblys.GetManifestResourceNames();
            //foreach (string resource in resources)
            //{
            //    Console.WriteLine(resource);
            //}

            // Load PDF template from a resource file
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = "";
            iText.Kernel.Pdf.PdfReader pdfReader;
            if (PDFPath=="")
            {
                switch (whatPDF)
                {
                    case "OF":
                        resourceName = "Mint.Resources.Order Template.pdf";
                        break;
                }

                using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
                }
            }
            else
            {
                pdfReader = new iText.Kernel.Pdf.PdfReader(PDFPath);
            }

            //Only do if pdfreader is not nothing
            if (pdfReader != null)
            { 
                string outputName = "output.pdf";
                int attempt = 1;
                while (File.Exists(outputName))
                {
                    //try to delete previous output.pdf
                    try
                    {
                        File.Delete(outputName);
                    }
                    catch
                    {
                        outputName = "output" + attempt + ".pdf";
                        attempt++;
                    }
                }
                iText.Kernel.Pdf.PdfWriter pdfWriter = new iText.Kernel.Pdf.PdfWriter(outputName);
                iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(pdfReader, pdfWriter);
                Document doc = new Document(pdfDoc);
                iText.Forms.PdfAcroForm form = iText.Forms.PdfAcroForm.GetAcroForm(pdfDoc, true);

                switch (whatPDF)
                {
                    case "OF":

                        //HEADERS
                        string orderQuery = "SELECT * FROM ViewOFOrder WHERE id_order = " + id;
                        DataTable dtOrder = Database.LoadData(orderQuery);
                        //Check if dtOrder is not empty (or if we ask for the template (will be id = 0)
                        if (id == "0") {; }
                        else if (dtOrder.Rows.Count == 0)
                        {
                            MessageBox.Show("Aucune commande avec cet ID");
                            pdfDoc.Close();
                            return;
                        }
                        

                        if (dtOrder.Rows.Count > 0)
                        {
                           
                            // Add a barcode to the PDF
                            var textField = form.GetField("Order_Barcode");
                            var rect = textField.GetWidgets()[0].GetRectangle();
                            float xPos = (float)rect.GetAsNumber(0).GetValue() + 100;
                            float yPos = (float)rect.GetAsNumber(1).GetValue();
                            Barcode128 barcode = new Barcode128(pdfDoc);
                            barcode.SetCode(id);
                            barcode.SetCodeType(Barcode128.CODE128);
                            Image barcodeImage = new Image(barcode.CreateFormXObject(pdfDoc));
                            barcodeImage.SetFixedPosition(xPos, yPos);
                            barcodeImage.ScaleToFit(70f, 70f);
                            doc.Add(barcodeImage);

                            //Loop through all columns of the database and see if there is a field corresponding with GetField in the PDF
                            foreach (DataColumn column in dtOrder.Columns)
                            {
                                string columnName = column.ColumnName;
                                // Check if a matching field exists in the PDF form
                                if (form.GetField(columnName) != null)
                                {
                                    form.GetField(columnName).SetValue(dtOrder.Rows[0][column].ToString());
                                }
                                else
                                {
                                    // Skips this field
                                }
                            }


                            //DETAILS
                            string salesQuery = "SELECT Référence, Type, Support, Dimension, Quantité, Poids, Statut FROM ViewOFSales WHERE id_order = " + id;
                            if (atelier != "") { salesQuery += " AND Atelier = '" + atelier + "'";}
                            DataTable dtSales = Database.LoadData(salesQuery);
                            //Check if dtOrder is not empty
                            if (dtSales.Rows.Count == 0)
                            {
                                string MessageNone = "Aucun objet à produire dans cette commande";
                                if (atelier != "") { MessageNone += " pour l'atelier " + atelier; }
                                MessageBox.Show(MessageNone);
                                return;
                            }
                            else
                            {
                                // Add the table
                                var tableStartField = form.GetField("Order_Table");
                                rect = tableStartField.GetWidgets()[0].GetRectangle();
                                // Calculate the column widths based on the size of the field
                                float fullwidth = (float)rect.GetAsNumber(2).GetValue() - (float)rect.GetAsNumber(0).GetValue();
                                var columnWidths = new float[] { 0.20f * fullwidth, 0.20f * fullwidth, 0.22f * fullwidth, 0.10f * fullwidth, 0.08f * fullwidth, 0.06f * fullwidth, 0.14f * fullwidth };


                                // Define a font with a specific size
                                float fontSize = 6; // Adjust the desired font size
                                float rowSize = 10; // Adjust the desired row size
                                PdfFont font = PdfFontFactory.CreateFont();
                            

                                // Create the table
                                var orderDetailsTable = new iText.Layout.Element.Table(columnWidths);

                                // Set table width to match the field's width
                                float fieldWidth = (float)rect.GetAsNumber(2).GetValue() - (float)rect.GetAsNumber(0).GetValue();
                                // Reduce the table width to fit the field if necessary
                                orderDetailsTable.SetWidth(UnitValue.CreatePointValue(fieldWidth));

                                xPos = (float)rect.GetAsNumber(0).GetValue();
                                yPos = (float)rect.GetAsNumber(1).GetValue();
                                float wPos = (float)rect.GetAsNumber(2).GetValue() - xPos;

                                // Adjust the row height to prevent word wrapping
                               orderDetailsTable.SetFixedLayout();

                                // Add headers with adjusted font size
                                foreach (DataColumn column in dtSales.Columns)
                                {
                                    var cell = new Cell().Add(new Paragraph(column.ColumnName).SetFont(font).SetFontSize(fontSize+1).SetBold());
                                    cell.SetHeight(rowSize);
                                    orderDetailsTable.AddHeaderCell(cell);
                                }

                                // Add rows with adjusted font size and row height
                                foreach (DataRow row in dtSales.Rows)
                                {
                                    foreach (DataColumn column in dtSales.Columns)
                                    {
                                        var cell = new Cell().Add(new Paragraph(row[column].ToString()).SetFont(font).SetFontSize(fontSize));
                                        cell.SetHeight(rowSize);
                                        orderDetailsTable.AddCell(cell);
                                    }
                                }
                            
                                float tableHeight = (dtSales.Rows.Count+1)*(rowSize+5);
                                orderDetailsTable.SetHeight(UnitValue.CreatePointValue(tableHeight));

                                // Add the table to the document
                                orderDetailsTable.SetFixedPosition(xPos, yPos-tableHeight+1, wPos);
                                doc.Add(orderDetailsTable);
                            }
                        }
                        break;                        
                }

                // Close the PDF document
                pdfDoc.Close();
                //Get the path of the PDF
                string outputFullPath = Path.GetFullPath(outputName);

                // open the PDF in the default application
                if (directPrint == true)
                {
                    //Save the PDF to the folder read by the printer : move the file to the folder and delete it
                    string PathToFolderForPrinter = Properties.Settings.Default.printerPath;

                    try
                    {
                        // Vérifie si le dossier de destination existe, sinon crée-le
                        if (!Directory.Exists(PathToFolderForPrinter))
                        {
                            MessageBox.Show("Pas de dossier renseigné pour stocker les OF à imprimer");
                            return;
                        }

                        // Construit le chemin complet de destination
                        string destinationFilePath = PathToFolderForPrinter + "\\" + whatPDF + '_' + id + '_' + outputName;

                        // Déplace le fichier PDF vers le dossier de destination
                        File.Move(outputFullPath, destinationFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la génération de l'OF : {ex.Message}");
                    }
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", outputFullPath);
                }
            }

        }

    }
}
