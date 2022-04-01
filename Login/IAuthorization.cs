using FotoGeoLocationWebApplication.Models;
using WebApplicationTurystyka.Models;

namespace FotoGeoLocationWebApplication.Login
{
    public interface IAuthorization
    {
        AuthorizedUserDto Login(LoginDto login);
    }
}