using Microsoft.Extensions.DependencyInjection;
using Rotativa_FullFramework.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wkhtmltopdf.NetCore;

namespace Rotativa_FullFramework.Controllers
{
    public class HomeController : Controller
    {
        IGeneratePdf _generatePdf;
        public HomeController()
        {
            var services = new ServiceCollection();
            services.AddWkhtmltopdf();
            _generatePdf = services.BuildServiceProvider().GetRequiredService<IGeneratePdf>();
        }

        public async Task<FileStreamResult> GetByHtml()
        {
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>This is a hardcoded test</h1>
                            </header>
                            <div>
                                <h2>456789</h2>
                            </div>
                        </body>";

            var pdf = _generatePdf.GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        [HttpGet]
        public async Task SaveByHtml()
        {
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>This is a hardcoded test</h1>
                            </header>
                            <div>
                                <h2>456789</h2>
                            </div>
                        </body>";

            System.IO.File.WriteAllBytes("testHard.pdf", _generatePdf.GetPDF(html));
        }

        [HttpGet]
        public async Task SaveFile()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            System.IO.File.WriteAllBytes("test.pdf", await _generatePdf.GetByteArray("Views/Test/Test.cshtml", data));
        }
    }
}
