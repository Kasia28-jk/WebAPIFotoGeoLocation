namespace FotoGeoLocationWebApplication.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public string Role { get; set; }
    }
}
