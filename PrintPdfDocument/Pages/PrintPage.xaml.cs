using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Printing;
using MessageBox = System.Windows.MessageBox;
using PdfiumViewer;
using System.IO;
using PrintPdfDocument.Controls;

namespace PrintPdfDocument
{
    /// <summary>
    /// PrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPage : Page
    {
        private Font printFont;
        private StreamReader streamToPrint;
        private string desktopPath;
        private List<Report> reports;
        public PrintPage()
        {
            InitializeComponent();
            reports = ReportHttp.GetReport();

            desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, System.Drawing.Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = this.desktopPath + @"\3c9908abf073412d1a9f1b853cf33658.jpg";
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (s, args) =>
                {
                    System.Drawing.Image i = System.Drawing.Image.FromFile(path);
                    System.Drawing.Rectangle m = args.PageBounds;
                    args.Graphics.DrawImage(i, m);
                };
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = this.desktopPath + @"\ReferenceCard.pdf";
            using (var document = PdfDocument.Load(path))
            {
                using (var printDocument = document.CreatePrintDocument())
                {
                    printDocument.PrinterSettings.PrintFileName = "Letter_SkidTags_Report_9ae93aa7-4359-444e-a033-eb5bf17f5ce6.pdf";
                    printDocument.PrinterSettings.PrinterName = @"OneNote";
                    printDocument.DocumentName = "file.pdf";
                    printDocument.PrinterSettings.PrintFileName = "file.pdf";
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                }
            }
        }
    }
}
