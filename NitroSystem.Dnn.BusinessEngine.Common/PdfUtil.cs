using DotNetNuke.Data;
using Microsoft.ApplicationBlocks.Data;
using NitroSystem.Dnn.BusinessEngine.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class PdfUtil
    {
        public static void HtmlToPdf(string html, string filename)
        {
            //var htmlToPdf = new HtmlToPdfConverter();
            //var pdfBytes = htmlToPdf.GeneratePdf(html);

            //using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(pdfBytes, 0, pdfBytes.Length);

            //    fs.Close();
            //}

            //htmlToPdf.GeneratePdfFromFile(url, null, "d:\\export.pdf");
        }
    }
}
