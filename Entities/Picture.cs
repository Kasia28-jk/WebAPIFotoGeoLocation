namespace FotoGeoLocationWebApplication.Entities
{
    public class Picture 
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
