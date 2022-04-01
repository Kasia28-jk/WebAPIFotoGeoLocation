using FotoGeoLocationWebApplication.Login;
using FotoGeoLocationWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebApplicationTurystyka.Models;

namespace FotoGeoLocationWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IAuthorization _logowanie;
        private readonly ILogger _logger;

        public LoginController(IAuthorization logowanie,
            ILogger<LoginController> logger)
        {
            _logowanie = logowanie;
            _logger = logger;
        }

        [HttpPost]
        public AuthorizedUserDto Login([FromBody] LoginDto login)
        {
            try
            { 
                return _logowanie.Login(login);
            }
            catch(Exception ex)
            {
                _logger.LogError("Błąd logowania! ");
                return new AuthorizedUserDto(); //puste to frontend ma wyczaic co jest zle 
                //dodac logowanie pozniej 
            }
        }
    }
}
