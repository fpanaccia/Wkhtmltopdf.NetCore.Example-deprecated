using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Rotativa_FullFramework.Models;
using Wkhtmltopdf.NetCore;

namespace Rotativa.Controllers
{
    public class TestDynamicViewsController : Controller
    {
        readonly IGeneratePdf _generatePdf;
        readonly string htmlView = @"@model Rotativa_FullFramework.Models.TestData
                        <!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>@Model.Text</h1>
                            </header>
                            <div>
                                <h2>@Model.Number</h2>
                            </div>
                        </body>
                        </html>";


        public TestDynamicViewsController()
        {
            var services = new ServiceCollection();
            services.AddWkhtmltopdf();
            var servicesProvider = services.BuildServiceProvider();
            _generatePdf = servicesProvider.GetRequiredService<IGeneratePdf>();
        }

        [HttpGet]
        [Route("GetByRazorText")]
        public async Task<IActionResult> GetByRazorText()
        {
            var data = new TestData
            {
                Text = "This is not a test",
                Number = 12345678
            };

            var pdf = await _generatePdf.GetPdfViewInHtml(htmlView, data);
            return pdf;
        }

        [HttpGet]
        [Route("GetByteArray")]
        public async Task<IActionResult> GetByteArray()
        {
            var data = new TestData
            {
                Text = "This is not a test",
                Number = 12345678
            };

            var pdf = await _generatePdf.GetByteArrayViewInHtml(htmlView, data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }


        [HttpGet]
        [Route("GetFromEngine")]
        public async Task<IActionResult> GetFromEngine()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            if(!_generatePdf.ExistsView("notAView"))
            {
                var html = System.IO.File.ReadAllText("Views/Test.cshtml");
                _generatePdf.AddView("notAView", html);
            }

            return await _generatePdf.GetPdf("notAView", data);
        }


        [HttpGet]
        [Route("UpdateAndGetFromEngine")]
        public async Task<IActionResult> UpdateAndGetFromEngine()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            if (!_generatePdf.ExistsView("notAView"))
            {
                var html = System.IO.File.ReadAllText("Views/Test.cshtml");
                _generatePdf.AddView("notAView", html);
            }
            else
            {
                var html = @"@model Rotativa_FullFramework.Models.TestData
                        <!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>@Model.Text</h1>
                            </header>
                            <div>
                                <h2>Repeat @Model.Text</h2>
                            </div>
                            <div>
                                <h5>@Model.Number</h2>
                            </div>
                        </body>
                        </html>";

                _generatePdf.UpdateView("notAView", html);
            }

            return await _generatePdf.GetPdf("notAView", data);
        }
    }
}
