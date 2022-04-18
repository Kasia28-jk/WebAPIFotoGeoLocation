using FotoGeoLocationWebApplication.Data;
using System;
using System.Linq;

namespace FotoGeoLocationWebApplication.Login
{
    public class Delete : IDelete
    {
        private readonly DataContext _dataContext;
        private readonly IHashProvider _encryptionProvider;

        public Delete(DataContext dataContext, IHashProvider encryptionProvider)
        {
            _dataContext = dataContext;
            _encryptionProvider = encryptionProvider;
        }

        public void DeleteUser(string password, int userId)
        {
            string hashed = _encryptionProvider.GetEncryptedPassword(password);

            var userLogin = _dataContext.Users.SingleOrDefault(x => x.Id.Equals(userId) &&
                                                                    x.Password.Equals(hashed));
            if (userLogin == null)
            {
                throw new Exception("Nie ma takie użytkownika!");
            }

            userLogin.Enabled = false;
            _dataContext.Users.Update(userLogin);
            _dataContext.SaveChanges();
        }
    }
}
