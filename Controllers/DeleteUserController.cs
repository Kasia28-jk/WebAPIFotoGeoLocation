using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using FotoGeoLocationWebApplication.Data;
using System.Linq;
using System;
using FotoGeoLocationWebApplication.Login;
using FotoGeoLocationWebApplication.Models;

namespace FotoGeoLocationWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteUserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IDelete _delete;
        private readonly DataContext _dataContext;

        public DeleteUserController(ILogger<DeleteUserController> logger,
            IDelete delete,
            DataContext dataContext)
        {
            _logger = logger;
            _delete = delete;
            _dataContext = dataContext;
        }

        [HttpPost]
        [Authorize]
        public bool DeleteUser(UserToRemoveDto password)
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            var token = authorizationToken.ToString();
            var session = _dataContext.Sessions.SingleOrDefault(x => x.Token.Equals(token)
                                                                 && x.ExpiresAt > DateTime.Now);
            try
            {
                _delete.DeleteUser(password.Password, session.UserId);
                return true;    
            }
            catch (Exception ex)
            {
                _logger.LogError("Błąd usuwanie użytkownika! ");
                return false;
            }
        }
    }
}
