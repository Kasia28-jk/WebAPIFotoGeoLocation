using System;
using System.Drawing;

namespace FotoGeoLocationWebApplication.GpsData
{
    public interface IExifDataExtractor
    {
        GpsData GetGpsData(Image image);
        DateTime GetDateTime(Image image);
    }
}