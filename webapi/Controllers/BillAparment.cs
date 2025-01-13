using iTextSharp.text;
using iTextSharp.text.pdf;
using webapi.Model;
using System.Net;
using SkiaSharp;
using PDFtoImage;

namespace webapi.Controllers
{
    public class BillAparment
    {
        public byte[] PrintBillPdf(Apartment item)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //string imgQrcodeUrl = @"https://drive.google.com/thumbnail?id=1TssFYH_nt4Zsl1uQVwCEtnxnDswbTDZJ&sz=w1000";
            string imgQrcodeUrl = @$"https://promptpay.io/0841127859/{item.total_amount}.png";
            string fontUrl = @"https://drive.google.com/uc?export=download&id=16xSMFxLGCAuhHwK2wzy6myAv68FqFlNL";
            byte[] fontData;
            byte[] pdfBytes;
            using (WebClient webClient = new WebClient())
            {
                fontData = webClient.DownloadData(fontUrl);
            }
            using (MemoryStream fontStream = new MemoryStream(fontData))
            {
                // Save the font locally (if required, optional step)
                string tempFontPath = Path.Combine(Path.GetTempPath(), "THSarabunNew.ttf");
                File.WriteAllBytes(tempFontPath, fontStream.ToArray());
                using (var memoryStream = new MemoryStream())
                {
                    // Create a Document object
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    try
                    {

                        // Create a memory stream to capture the PDF output
                        // PDF writer setup to write to memory stream
                        document.Open();

                        // Create the base font using the correct path
                        BaseFont baseFont = BaseFont.CreateFont(tempFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        // Create fonts using the embedded Thai font
                        Font titleFont = new Font(baseFont, 14, Font.BOLD);
                        Font headerFont = new Font(baseFont, 12, Font.BOLD);
                        Font tableHeaderFont = new Font(baseFont, 10, Font.BOLD);
                        Font normalFont = new Font(baseFont, 10, Font.NORMAL);

                        // Header: Title and Address
                        Paragraph title = new Paragraph("ใบแจ้งค่าหอพัก", titleFont) { Alignment = Element.ALIGN_CENTER };
                        document.Add(title);

                        Paragraph address = new Paragraph("หอพักสวนปาล์ม\n115/5 - 115/12 ตำบลหนองบอนแดง, อำเภอบ้านบึง, จังหวัดชลบุรี 20170\nโทร 084-1127-859", normalFont) { Alignment = Element.ALIGN_CENTER };
                        address.SpacingAfter = 10f;
                        document.Add(address);

                        // Rental Information
                        PdfPTable infoTable = new PdfPTable(7) { WidthPercentage = 100 };
                        infoTable.SetWidths(new float[] { 0.5F, 3F, 1.5F, 1.5F, 1.5F, 1.5F, 1.5F });
                        infoTable.AddCell(new PdfPCell(new Phrase($"ห้องพัก: {item.room_number}", normalFont))
                        {
                            Colspan = 5,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            FixedHeight = 30f
                        });
                        infoTable.AddCell(new PdfPCell(new Phrase($"ประจำเดือน\n {item.Month_TH}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        infoTable.AddCell(new PdfPCell(new Phrase($"เลขที่ No: {item.bill_id}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        document.Add(infoTable);
                        // Table: Itemized Details
                        PdfPTable table = new PdfPTable(7) { WidthPercentage = 100 };
                        table.SetWidths(new float[] { 0.5F, 3F, 1.5F, 1.5F, 1.5F, 1.5F, 1.5F });

                        // Table Header
                        table.AddCell(new PdfPCell(new Phrase("ลำดับ\nItem", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("รายการ\nDescription", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("ค่าครั้งก่อน\n(หน่วย)", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("ค่าปัจจุบัน\n(หน่วย)", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("จำนวนหน่วยที่ใช้\n(หน่วย)", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("ราคาต่อหน่วย\n(บาท)", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("จำนวนเงิน\n(บาท)", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        // Table Rows (Add your logic for item rows here)
                        table.AddCell(new PdfPCell(new Phrase("1", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("ค่าห้องเช่า", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.room_rent}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        // Table Row 2 
                        table.AddCell(new PdfPCell(new Phrase("2", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("ค่าน้ำ", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.previous_meter_month}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.water_reading_meter}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.water_diff}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.water_unit_fees}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //Calculated Water_Fees
                        int water_fees = item.water_diff * item.water_unit_fees;
                        table.AddCell(new PdfPCell(new Phrase($"{water_fees}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        // Table Row 3
                        table.AddCell(new PdfPCell(new Phrase("3", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("ค่าขยะ", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.garbage_fees}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        // Table Row 4
                        table.AddCell(new PdfPCell(new Phrase("4", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("อื่นๆ", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase("", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.other_fees}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        // Total Amount
                        PdfPCell totalCell = new PdfPCell(new Phrase($"เป็นจำนวนเงิน {item.BAHT}", normalFont)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f };
                        table.AddCell(totalCell);
                        table.AddCell(new PdfPCell(new Phrase("จำนวนเงินรวม", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        table.AddCell(new PdfPCell(new Phrase($"{item.total_amount}", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                        // Remarks Cell
                        PdfPCell remarkCell = new PdfPCell(new Phrase("หมายเหตุ: กำหนดการชำระเงินภายในวันที่ 1 - 5 ของแต่ละเดือน หากเกินกำหนดจะมีการปรับวันละ 100 บาท", normalFont))
                        {
                            Colspan = 7,
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            FixedHeight = 20f
                        };
                        table.AddCell(remarkCell);

                        table.SpacingAfter = 20f;
                        document.Add(table);
                        // imageQRCode
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imgQrcodeUrl);

                        // Set image properties (optional)
                        image.ScaleToFit(150f, 150f); // Scale the image to fit within 100x100 points
                        image.Alignment = Element.ALIGN_CENTER; // Center the image
                        image.SpacingBefore = 20f; // Add space before the image
                        image.SpacingAfter = 20f; // Add space after the image
                        document.Add(image);
                        Paragraph accountBanking = new Paragraph("นางไพพร เหลืองชะนะ", normalFont) { Alignment = Element.ALIGN_CENTER };
                        document.Add(accountBanking);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error generating PDF: " + ex.Message);
                    }
                    finally
                    {
                        document.Close();
                    }

                    pdfBytes = memoryStream.ToArray();
                }
                return pdfBytes;
            }
        }
    }
}


