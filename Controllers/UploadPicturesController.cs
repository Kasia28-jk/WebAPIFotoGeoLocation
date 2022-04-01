using FotoGeoLocationWebApplication.GpsData;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadPicturesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGpsDataExtractor _gpsDataExtractor;
        private readonly ILogger _logger;

        public UploadPicturesController(IWebHostEnvironment webHostEnvironment,
            IGpsDataExtractor gpsDataExtractor,
            ILogger<UploadPicturesController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
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
                var path = Path.Combine(_webHostEnvironment.ContentRootPath,
                    "UploadedPictures",
                    $"{guid}{fileName}");

                using(var f = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(f);
                }

                var image = Image.FromFile(path);

                try
                {
                    var gpsData = _gpsDataExtractor.GetGpsData(image);
                    image.Dispose();
                }
                catch (Exception ex) //moze stworzyc wlasny exception? we will see about that 
                {
                    image.Dispose();
                    _logger.LogWarning($"Received file doesn't contain GPS data! {ex.Message}");
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
