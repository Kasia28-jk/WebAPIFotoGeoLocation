namespace FotoGeoLocationWebApplication.Entities
{
    public class Picture // jak zapisac dane zdjecie? 
        //sciezka do dysku? czy jako blob? czyli zmienna tpyu binarnego
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public bool IsMain { get; set; }
        public int UserId { get; set; }
    }
}
