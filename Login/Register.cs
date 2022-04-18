using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Entities;
using FotoGeoLocationWebApplication.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FotoGeoLocationWebApplication.Login
{
    public class Register : IRegister
    {
        private readonly DataContext _dataContext;
        private readonly IHashProvider _encryptionProvider;
        private readonly ILogger _logger;

        public Register(DataContext dataContext,
            IHashProvider encryptionProvider,
            ILogger<Register> logger)
        {
            _dataContext = dataContext;
            _encryptionProvider = encryptionProvider;
            _logger = logger;
        }

        public void RegisterUser(RegisterDto register)
        {
            if (register.Login == null) //Przetestowac czy jest wg mozliwe?
            {
                _logger.LogError("Dane użytkownika są nullowe! ");
                throw new System.Exception();
            }

            if (_dataContext.Users.Any(x => x.UserName.Equals(register.Login) && x.Enabled.Equals(true)))
            {
                _logger.LogError("Istnieje użytkownik o takim loginie");
                throw new System.Exception();
            }

            string hashed = _encryptionProvider.GetEncryptedPassword(register.Password);

            var user = new User
            {
                UserName = register.Login,
                Password = hashed,
                Enabled = true
            };

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }
    }
}
