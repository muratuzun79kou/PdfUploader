using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfUploeder.Models;
using System.IO;

namespace PdfUploeder.Controllers
{
    public class Files1Controller : Controller
    {
        IHostingEnvironment _hostingEnvironment = null;
        public Files1Controller(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string fileName="")
        {
            FileClass fileObj = new FileClass();
            fileObj.Name=fileName;
            string path = $"{_hostingEnvironment.WebRootPath}\\files\\";
            int nId = 1;
            foreach (string pdfPath in Directory.EnumerateFiles(path,"*.pdf"))
            {
                fileObj.Files.Add(new FileClass()
                {
                    FileId =nId++,
                    Name = Path.GetFileName(pdfPath),
                    Path = pdfPath
                });
            }

            return View(fileObj);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            return Index();
        }
    }
}
