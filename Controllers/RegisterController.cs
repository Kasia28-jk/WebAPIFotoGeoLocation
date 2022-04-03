using FotoGeoLocationWebApplication.Login;
using FotoGeoLocationWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FotoGeoLocationWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IRegister _register;
        private readonly ILogger _logger;

        public RegisterController(IRegister register, ILogger<RegisterController> logger)
        {
            _register = register;
            _logger = logger;
        }

        [HttpPost]
        public bool RegisterUser([FromBody] RegisterDto register)
        {
            try
            {
                _register.RegisterUser(register);
            }
            catch
            {
                _logger.LogWarning("Nie można stworzyć użytkownika!");
                return false;
            }

            return true;
        }
    }
}
