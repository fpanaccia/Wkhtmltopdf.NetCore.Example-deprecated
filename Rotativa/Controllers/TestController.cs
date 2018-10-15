using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa.Models;
using Wkhtmltopdf.NetCore;

namespace Rotativa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var builder = new ViewAsPdf();
            return await builder.GetPdf("Views/Test.cshtml", data);
        }
        
        [HttpGet]
        [Route("GetByHtml")]
        public async Task<IActionResult> GetByHtml()
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

            var builder = new HtmlAsPdf();
            var pdf = builder.GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        [HttpGet]
        [Route("SaveByHtml")]
        public async Task<IActionResult> SaveByHtml()
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

            var builder = new HtmlAsPdf();
            System.IO.File.WriteAllBytes("testHard.pdf", builder.GetPDF(html));
            return Ok();
        }
        
        [HttpGet]
        [Route("SaveFile")]
        public async Task<IActionResult> SaveFile()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var builder = new ViewAsPdf();
            System.IO.File.WriteAllBytes("test.pdf", await builder.GetByteArray("Views/Test.cshtml", data));
            return Ok();
        }

    }
}
