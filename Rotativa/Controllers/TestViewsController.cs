using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa.Models;
using Wkhtmltopdf.NetCore;

namespace Rotativa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestViewsController : ControllerBase
    {
        readonly IGeneratePdf _generatePdf;
        public TestViewsController(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        /// <summary>
        /// View pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSSL")]
        public async Task<IActionResult> GetSSL()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            return await _generatePdf.GetPdf("Views/TestBootstrapSSL.cshtml", data);
        }

        /// <summary>
        /// View pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            return await _generatePdf.GetPdf("Views/Test.cshtml", data);
        }

        /// <summary>
        /// View pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByteArray")]
        public async Task<IActionResult> GetByteArray()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var pdf = await _generatePdf.GetByteArray("Views/Test.cshtml", data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByHtml")]
        public IActionResult GetByHtml()
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
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray and save file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveByHtml")]
        public IActionResult SaveByHtml()
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
            return Ok();
        }

        /// <summary>
        /// View pdf generation as ByteArray and save file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveFile")]
        public async Task<IActionResult> SaveFile()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            System.IO.File.WriteAllBytes("test.pdf", await _generatePdf.GetByteArray("Views/Test.cshtml", data));
            return Ok();
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray with Header
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("HeaderTest")]
        public async Task<IActionResult> HeaderTest()
        {
            var options = new CustomOptions
            {
                HeaderHtml = "http://localhost/header.html",
                PageOrientation = Wkhtmltopdf.NetCore.Options.Orientation.Landscape
            };
            _generatePdf.SetConvertOptions(options);

            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var pdf = await _generatePdf.GetByteArray("Views/Test.cshtml", data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray with Header
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MarginTest")]
        public async Task<IActionResult> MarginTest()
        {
            var options = new ConvertOptions
            {
                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins()
                {
                    Left = 0,
                    Right = 0
                }
            };

            _generatePdf.SetConvertOptions(options);

            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var pdf = await _generatePdf.GetByteArray("Views/Test.cshtml", data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray with Header
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("HeaderPagingTest")]
        public IActionResult HeaderPagingTest()
        {
            var options = new ConvertOptions
            {
                HeaderHtml = "http://localhost/header.html"
            };
            _generatePdf.SetConvertOptions(options);

            string htmlCode = "";
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://www.spacex.com/vehicles/starship/");
            }

            var pdf = _generatePdf.GetPDF(htmlCode);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray with Footer and Replacements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("FooterTest")]
        public async Task<IActionResult> FooterTest()
        {
            var kv = new Dictionary<string, string>
            {
                { "username", "Veaer" },
                { "age", "20" },
                { "url", "google.com" }
            };

            var options = new ConvertOptions
            {
                FooterHtml = "http://localhost/footer.html",
                Replacements = kv
            };
            _generatePdf.SetConvertOptions(options);

            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var pdf = await _generatePdf.GetByteArray("Views/Test.cshtml", data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }


        /// string form view pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFormByteArray")]
        public async Task<IActionResult> GetFormByteArray()
        {
            var options = new ConvertOptions
            {
                EnableForms = true
            };

            _generatePdf.SetConvertOptions(options);

            var pdf = await _generatePdf.GetByteArray("Views/Test_Form.cshtml");
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }
    }
}
