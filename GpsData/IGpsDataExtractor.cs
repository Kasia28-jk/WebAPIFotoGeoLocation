using System.Drawing;

namespace FotoGeoLocationWebApplication.GpsData
{
    public interface IGpsDataExtractor
    {
        GpsData GetGpsData(Image image);
    }
}