using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Models;
using FotoGeoLocationWebApplication.Pictures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureProviderController : Controller
    {
        private readonly IPictureProvider _pictureProvider;
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;

        public PictureProviderController(IPictureProvider pictureProvider,
            ILogger<PictureProviderController> logger,
            DataContext dataContext)
        {
            _pictureProvider = pictureProvider;
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<PictureDto> GetPictures()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            var token = authorizationToken.ToString();
            var session = _dataContext.Sessions.SingleOrDefault(x => x.Token.Equals(token)
                                                                 && x.ExpiresAt > DateTime.Now);

            return _pictureProvider.GetPictures(session.UserId);
        }
    }
}
