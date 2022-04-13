using FotoGeoLocationWebApplication.Models;
using FotoGeoLocationWebApplication.Pictures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureProviderController : Controller
    {
        private readonly IPictureProvider _pictureProvider;
        private readonly ILogger _logger;

        public PictureProviderController(IPictureProvider pictureProvider,
            ILogger<PictureProviderController> logger)
        {
            _pictureProvider = pictureProvider;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<PictureDto> GetPictures()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            var token = authorizationToken.ToString();

            return _pictureProvider.GetPictures(token);
        }
    }
}
