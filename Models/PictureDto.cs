﻿using Microsoft.AspNetCore.Http;

namespace FotoGeoLocationWebApplication.Models
{
    public class PictureDto
    {
        public string File { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserName { get; set; }
    }
}
