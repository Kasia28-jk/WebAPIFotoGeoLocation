using System;

namespace FotoGeoLocationWebApplication.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
