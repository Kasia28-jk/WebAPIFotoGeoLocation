using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Drawing;
using FotoGeoLocationWebApplication.GpsData;
using Microsoft.Extensions.Logging;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadPicturesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGpsDataExtractor _gpsDataExtractor;
        private readonly ILogger _logger;

        public UploadPicturesController(IWebHostEnvironment env, IGpsDataExtractor gpsDataExtractor, ILogger<UploadPicturesController> logger)
        {
            _env = env;
            _gpsDataExtractor = gpsDataExtractor;
            _logger = logger;
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

                try
                {
                    var gpsData = _gpsDataExtractor.GetGpsData(image);
                    image.Dispose();
                }
                catch (Exception ex)
                {
                    image.Dispose();
                    _logger.LogWarning("Received file doesn't contain GPS data!");
                    System.IO.File.Delete(path);
                    return "failed";
                }
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
