using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;

namespace FotoGeoLocationWebApplication.GpsData
{
    public class GpsDataExtractor : IGpsDataExtractor
    {
        private readonly ILogger _logger;
        public GpsDataExtractor(ILogger<GpsDataExtractor> logger)
        {
            _logger = logger;
        }
        public GpsData GetGpsData(Image image)
        {
            var latitude = GetLatitudeCoefficient(image) * GetLatitudeValue(image);
            var longitude = GetLongitudeCoefficient(image) * GetLongitudeValue(image);
            return new GpsData()
            {
                latitude = latitude,
                longitude = longitude,
            };
        }

        private double GetLatitudeValue(Image image)
        {
            const int latitudeIndex = 2;
            try
            {
                var GpsLatitude = image.GetPropertyItem(latitudeIndex).Value; //szerokosc
                return GetGpsValue(GpsLatitude);
            }
            catch (ArgumentException arg)
            {
                _logger.LogError("Zdjęcie nie zawiera danych GPS.", arg.Message);
                throw;
            }
        }

        private double GetLatitudeCoefficient(Image image)
        {
            const int latitudeDirectionIndex = 1;
            const byte northChar = 78; //(byte)'N'
            byte[] GpsLatitudeDirection;
            try
            {
                GpsLatitudeDirection = image.GetPropertyItem(latitudeDirectionIndex).Value;
            }
            catch (ArgumentException arg)
            {
                _logger.LogError("Zdjęcie nie zawiera danych GPS.", arg.Message);
                throw;
            }

            if (GpsLatitudeDirection[0] == northChar)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private double GetLongitudeValue(Image image)
        {
            const int longitudeIndex = 4;
            try
            {
                var GpsLongitude = image.GetPropertyItem(longitudeIndex).Value; //dlugosc
                return GetGpsValue(GpsLongitude);
            }
            catch (ArgumentException arg)
            {
                _logger.LogError("Zdjęcie nie zawiera danych GPS.", arg.Message);
                throw;
            }
        }

        private double GetLongitudeCoefficient(Image image)
        {
            const int longitudeDirectionIndex = 3;
            const byte eastChar = 69; //(byte)'E'
            byte[] GpsLongitudeDirection;
            try
            {
                GpsLongitudeDirection = image.GetPropertyItem(longitudeDirectionIndex).Value;
            }
            catch (ArgumentException arg)
            {
                _logger.LogError("Zdjęcie nie zawiera danych GPS.", arg.Message);
                throw;
            }

            if (GpsLongitudeDirection[0] == eastChar)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private double GetGpsValue(byte[] data)
        {
            using (var br = new BinaryReader(new MemoryStream(data)))
            {
                var degreeNumerator = br.ReadUInt32();
                var degreeDenominator = br.ReadUInt32();
                var minuteNumerator = br.ReadUInt32();
                var minuteDenominator = br.ReadUInt32();
                var secondNumerator = br.ReadUInt32();
                var secondDenominator = br.ReadUInt32();
                return (double)degreeNumerator / degreeDenominator +
                    (double)minuteNumerator / minuteDenominator / 60.0 +
                    (double)secondNumerator / secondDenominator / 3600.0;
            }
        }
    }
}
