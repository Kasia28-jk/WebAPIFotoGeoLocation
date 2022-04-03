namespace FotoGeoLocationWebApplication.Entities
{
    public class Picture // jak zapisac dane zdjecie? 
        //sciezka do dysku? czy jako blob? czyli zmienna tpyu binarnego
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int UserId { get; set; }
    }
}
