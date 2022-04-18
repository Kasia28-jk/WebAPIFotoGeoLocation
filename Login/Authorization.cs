using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Entities;
using FotoGeoLocationWebApplication.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApplicationTurystyka.Models;

namespace FotoGeoLocationWebApplication.Login
{
    public class Authorization : IAuthorization
    {
        private readonly DataContext _dataContext;
        private readonly IHashProvider _encryptionProvider;

        public Authorization(DataContext dataContext, IHashProvider encryptionProvider)
        {
            _dataContext = dataContext;
            _encryptionProvider = encryptionProvider;
        }

        public AuthorizedUserDto Login(LoginDto login)
        {
            var res = new AuthorizedUserDto();

            string hashed = _encryptionProvider.GetEncryptedPassword(login.Password);

            var userLogin = _dataContext.Users.SingleOrDefault(x => x.UserName.Equals(login.Login) &&
                                                                    x.Password.Equals(hashed) &&
                                                                    x.Enabled.Equals(true));

            if (userLogin == null)
            {
                throw new Exception("Błędny login lub hasło!");
            }

          // userLogin.Enabled = true;
         //   _dataContext.Users.Update(userLogin);
          //  _dataContext.SaveChanges();
            res.Role = "User";
            /* Console.WriteLine("login:" + login.Login);
             Console.WriteLine("Haslo:" + login.Haslo);
             if (login.Login == "admin" && login.Haslo == "admin")
             {
                 res.Rola = "Admin";
             }
             else if (login.Login == "user" && login.Haslo == "user")
             {
                 res.Rola = "User";
             }
             else
             {
                 throw new Exception("Błędny login lub hasło!");
             }*/
            var userSesion = _dataContext.Sessions.SingleOrDefault(x => x.UserId.Equals(userLogin.Id));
            
            if (userSesion == null || userSesion.ExpiresAt < DateTime.Now)
            {
                var expires = DateTime.Now.AddMinutes(30);

                res.Token = GetToken(res.Role, expires);
                var tokenWithScheme = "Bearer " + res.Token;

                var session = new Session()
                {
                    UserId = userLogin.Id,
                    Token = tokenWithScheme,
                    ExpiresAt = expires
                };

                _dataContext.Sessions.Add(session);
                _dataContext.SaveChanges();
            }
            else
            {
                var expires = DateTime.Now.AddMinutes(30);

                res.Token = GetToken(res.Role, expires);
                var tokenWithScheme = "Bearer " + res.Token;

                userSesion.ExpiresAt = expires;
                userSesion.Token = tokenWithScheme;

                _dataContext.Sessions.Update(userSesion);
                _dataContext.SaveChanges();
            }
            return res;
        }

        private string GetToken(string role, DateTime expires)
        {
            var klucz = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bardzotrudnehaslotokena"));
            var zaszfrowanyKlucz = new SigningCredentials(klucz, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("http://localhost:45455", null, new List<Claim> { new Claim(ClaimTypes.Role, role) }, null, expires, zaszfrowanyKlucz);
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
