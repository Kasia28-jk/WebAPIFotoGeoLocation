namespace FotoGeoLocationWebApplication.Login
{
    public interface IEncryptionProvider
    {
        string GetEncryptedPassword(string login);
    }
}