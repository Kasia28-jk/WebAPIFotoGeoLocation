namespace FotoGeoLocationWebApplication.Login
{
    public interface IHashProvider
    {
        string GetEncryptedPassword(string login);
    }
}