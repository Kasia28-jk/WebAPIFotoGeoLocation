using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Drawing;
using FotoGeoLocationWebApplication.GpsData;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadPicturesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGpsDataExtractor _gpsDataExtractor;

        public UploadPicturesController(IWebHostEnvironment env, IGpsDataExtractor gpsDataExtractor)
        {
            _env = env;
            _gpsDataExtractor = gpsDataExtractor;
        }

        [EnableCors("AllowMyOrigin")]
        [HttpPost, DisableRequestSizeLimit]
        public string Post(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var guid = Guid.NewGuid();
                var path = Path.Combine(_env.ContentRootPath, "UploadedPictures", $"{guid}{fileName}");
                var f = new FileStream(path, FileMode.Create);
                file.CopyTo(f);
                f.Flush();
                f.Close();
                f.Dispose();

                var image = Image.FromFile(path);
                var gpsData = _gpsDataExtractor.GetGpsData(image);
            }
            return "test";
        }

        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public string get()
        {
            return "Hello world !";
        }
    }
}
