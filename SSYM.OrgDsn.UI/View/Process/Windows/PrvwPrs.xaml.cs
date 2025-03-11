using System.IO;
using System.Reflection;
using Microsoft.Win32;
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
using System.Windows.Shapes;
using SSYM.OrgDsn.ViewModel.Utility;
using yWorks.yFiles.UI;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.UI.View.Process.Windows
{
    /// <summary>
    /// Interaction logic for PrvwPrs.xaml
    /// </summary>
    public partial class PrvwPrs : Window
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        private Stream temp = null;
        private GraphControl grphCtl = null;
        private string PrsName = "";
        public PrvwPrs(GraphControl dgm, double w, double h,string Prsnam)
        {
            
            InitializeComponent();
            this.Width = w;
            this.Height = h;
            temp = new MemoryStream();
            dgm.ExportToBitmap(temp, "image/png", dgm.ContentRect);
            GraphImage.Source = BitmapFrame.Create(temp,
                                      BitmapCreateOptions.None,
                                      BitmapCacheOption.OnLoad);
            grphCtl = dgm;
            PrsName = Prsnam;
        }

        #endregion

        #region ' Properties / Commands '

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private void export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (.pdf)|*.pdf";
            save.DefaultExt = ".pdf";
            save.FileName = "Untitled";
            save.RestoreDirectory = true;
            save.Title = "ذخیره";
            save.CheckPathExists = false;
            if (save.ShowDialog() == true)
            {
                //PdfExporter pdf = new PdfExporter();
                CreatePdf(save.FileName);
                //pdf.Export(Dgm, save.FileName);
            }
        }

        /// <summary>
        /// Takes a collection of BMP files and converts them into a PDF document
        /// </summary>
        /// <param name="bmpFilePaths"></param>
        /// <returns></returns>
        private void CreatePdf(string FilePath)
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            int dpiX ;
            int dpiY ;
            if (dpiXProperty != null && dpiYProperty != null)
            {
                dpiX = (int) dpiXProperty.GetValue(null, null);
                dpiY = (int) dpiYProperty.GetValue(null, null);
            }
            else
            {
                dpiX = 72;
                dpiY = 72;
            }
            // load the image and count the total pages  
            System.Drawing.Bitmap bm = System.Drawing.Bitmap.FromStream(temp) as System.Drawing.Bitmap;

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, dpiX/2, dpiX/2,
                dpiY/2, dpiY/2);
            // creation of the different writers  
            
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(FilePath, System.IO.FileMode.Create));

            
            int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

            document.Open();
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            for (int k = 0; k < total; ++k)
            {
                bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Png);
                // scale the image to fit in the page  
                float Wpercent = (iTextSharp.text.PageSize.A4.Height - dpiX) / img.Height;
                float Hpercent = (iTextSharp.text.PageSize.A4.Width - dpiY) / img.Width;
                img.ScalePercent(((Wpercent < Hpercent ? Wpercent : Hpercent) > 1 ? 1 : (Wpercent < Hpercent ? Wpercent : Hpercent)) * 100);
                img.SetAbsolutePosition(dpiX / 2, (iTextSharp.text.PageSize.A4.Height - dpiX / 2) - (img.Height * ((Wpercent < Hpercent ? Wpercent : Hpercent) > 1 ? 1 : (Wpercent < Hpercent ? Wpercent : Hpercent))));
                cb.AddImage(img);
                document.NewPage();
            }
            document.Close(); 
        }

        #endregion

        #region ' events '

        #endregion

        private void XPDLExport(object sender, RoutedEventArgs e)
        {
            var stream = new MemoryStream();
            grphCtl.ExportToGraphML(stream);
            var aray = stream.ToArray();
            string str1 = Encoding.UTF8.GetString(aray);
            var save = new SaveFileDialog();
            save.FileName = "Untitled";
            save.DefaultExt = ".XPDL";
            save.Title = "ذخیره";
            save.RestoreDirectory = true;
            save.Filter = "Process Definition Language (.XPDL)|*.XPDL";
            bool? result = save.ShowDialog();
            if (result == true)
            {
                var generator = new XPDLGenerator(str1, save.FileName,PrsName );
            }
        }
    }
}
