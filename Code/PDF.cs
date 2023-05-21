using iText.Barcodes;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Image = iText.Layout.Element.Image;

namespace Mint.Code
{
    internal class PDF
    {
        public static void CreatePDF()
        {
            System.Reflection.Assembly assemblys = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resources = assemblys.GetManifestResourceNames();
            foreach (string resource in resources)
            {
                Console.WriteLine(resource);
            }

            // Load PDF template from a resource file
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = "Mint.Resources.Order Template.pdf";

            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
            {



                // Create a PDF reader object
                iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);

                // Create a PDF writer object
                iText.Kernel.Pdf.PdfWriter pdfWriter = new iText.Kernel.Pdf.PdfWriter("output.pdf");

                // Create a PDF document object
                iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(pdfReader, pdfWriter);

                // Define the Doc part of that PDFDOC
                Document doc = new Document(pdfDoc);

                // Get the PDF form
                iText.Forms.PdfAcroForm form = iText.Forms.PdfAcroForm.GetAcroForm(pdfDoc, true);

                // Modify the textboxes
                form.GetField("Order_Date").SetValue("2023-05-15");
                form.GetField("Client_Name").SetValue("John Smith");

                // Add a barcode to the PDF
                var textField = form.GetField("Order_Barcode");
                var rect = textField.GetWidgets()[0].GetRectangle();

                float xPos = (float)rect.GetAsNumber(0).GetValue()+50;
                float yPos = (float)rect.GetAsNumber(1).GetValue();


                Barcode128 barcode = new Barcode128(pdfDoc);
                barcode.SetCode("1234");
                barcode.SetCodeType(Barcode128.CODE128);
                Image barcodeImage = new Image(barcode.CreateFormXObject(pdfDoc));


                barcodeImage.SetFixedPosition(xPos, yPos);
                barcodeImage.ScaleToFit(100f, 100f);
                doc.Add(barcodeImage);


                // Add the table
                // Get the table start field
                var tableStartField = form.GetField("Order_Table");
                // Get the position and size of the field
                rect = tableStartField.GetWidgets()[0].GetRectangle();
                // Calculate the column widths based on the size of the field
                float fullwidth = (float)rect.GetAsNumber(2).GetValue() - (float)rect.GetAsNumber(0).GetValue();
                var columnWidths = new float[] {0.15f * fullwidth, 0.25f * fullwidth, 0.25f * fullwidth, 0.15f * fullwidth, 0.20f * fullwidth};

                // Create the table using the calculated column widths
                var orderDetailsTable = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnWidths));
                xPos = (float)rect.GetAsNumber(0).GetValue();
                yPos = (float)rect.GetAsNumber(1).GetValue();
                float wPos = (float)rect.GetAsNumber(2).GetValue()-xPos;


                // Add headers
                orderDetailsTable.AddHeaderCell("ID");
                orderDetailsTable.AddHeaderCell("Item Name");
                orderDetailsTable.AddHeaderCell("Description");
                orderDetailsTable.AddHeaderCell("Quantity");
                orderDetailsTable.AddHeaderCell("Price");

                // add rows
                orderDetailsTable.AddCell("1");
                orderDetailsTable.AddCell("Item 1");
                orderDetailsTable.AddCell("Description 1");
                orderDetailsTable.AddCell("2");
                orderDetailsTable.AddCell("$10.00");

                orderDetailsTable.AddCell("2");
                orderDetailsTable.AddCell("Item 2");
                orderDetailsTable.AddCell("Description 2");
                orderDetailsTable.AddCell("1");
                orderDetailsTable.AddCell("$5.00");


                // Calculate the height of the table
                // TODO: fix the placement of the table on the PDF based on the table's rows and row height

                float tableHeight = 3*22;

                // Add the table element to the document
                orderDetailsTable.SetFixedPosition(xPos, yPos - tableHeight, wPos);
                doc.Add(orderDetailsTable);

                // Close the PDF document
                pdfDoc.Close();

                // open the PDF in the default application
                System.Diagnostics.Process.Start("explorer.exe","C:\\Users\\Simon\\OneDrive\\Documents\\ExcelTab\\ExcelTab\\ERP\\Mint\\bin\\Debug\\net6.0-windows\\output.pdf") ;
            }

        }

    }
}
