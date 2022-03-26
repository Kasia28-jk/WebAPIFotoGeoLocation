using Microsoft.AspNetCore.Http;
using System;

namespace FotoGeoLocationWebApplication.Models
{
    public class UploadPicturesDto
    {
        public IFormFile File { get; set; }
        public DateTime Data { get; set; }
    }
}
