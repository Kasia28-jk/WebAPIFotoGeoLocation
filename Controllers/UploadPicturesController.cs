using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Entities;
using FotoGeoLocationWebApplication.GpsData;
using FotoGeoLocationWebApplication.Pictures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadPicturesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGpsDataExtractor _gpsDataExtractor;
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly IThumbnailGenerator _thumbnailGenerator;

        public UploadPicturesController(IWebHostEnvironment webHostEnvironment,
            IGpsDataExtractor gpsDataExtractor,
            ILogger<UploadPicturesController> logger,
            DataContext dataContext,
            IThumbnailGenerator thumbnailGenerator)
        {
            _webHostEnvironment = webHostEnvironment;
            _gpsDataExtractor = gpsDataExtractor;
            _logger = logger;
            _dataContext = dataContext;
            _thumbnailGenerator = thumbnailGenerator;
        }

        [EnableCors("AllowMyOrigin")]
        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public UploadPictureResponse Post(IFormFile file)
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            var token = authorizationToken.ToString();
            var session = _dataContext.Sessions.SingleOrDefault(x => x.Token.Equals(token) 
                                                                    && x.ExpiresAt > DateTime.Now);
            if(session == null)
            {
                return null;
            }

            if (file == null || file.Length == 0)
            {
                _logger.LogError($"Plik {file.Name} jest pusty!");
                return new UploadPictureResponse()
                {
                    Status = "failed"
                };
            }

            var fileName = Path.GetFileName(file.FileName);
            var guid = Guid.NewGuid();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath,
                "UploadedPictures",
                $"{guid}{fileName}");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var image = Image.FromFile(path);
            try
            {
                var gpsData = _gpsDataExtractor.GetGpsData(image);
                var imageData = _thumbnailGenerator.GetThumbnailImageData(image, 500, 500);
                image.Dispose();
                image = null;
                System.IO.File.Delete(path);
                System.IO.File.WriteAllBytes(path, imageData);
        
                var picture = new Picture()
                {
                    Path = path,
                    Latitude = gpsData.latitude,
                    Longitude = gpsData.longitude,
                    UserId = session.UserId,
                };

                _dataContext.Pictures.Add(picture);
                _dataContext.SaveChanges();
            }
            catch (Exception ex) //moze stworzyc wlasny exception? we will see about that 
            {
                if(image != null)
                {
                    image.Dispose();
                }
                _logger.LogWarning($"Otrzymane zdjęcie nie zawiera danych GPS! {ex.Message}");
                System.IO.File.Delete(path);
                return new UploadPictureResponse()
                {
                    Status = "failed"
                };
            }

            _logger.LogInformation($"Zdjęcie {path} zostało zapisane w bazie danych");
            return new UploadPictureResponse()
            {
                Status = "ok"
            };
        }

        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public string get()
        {
            return "Hello world !";
        }
    }
}
