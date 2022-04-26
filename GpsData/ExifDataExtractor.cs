using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace FotoGeoLocationWebApplication.GpsData
{
    public class ExifDataExtractor : IExifDataExtractor
    {
        private readonly ILogger _logger;
        public ExifDataExtractor(ILogger<ExifDataExtractor> logger)
        {
            _logger = logger;
        }

        public GpsData GetGpsData(Image image)
        {
            var latitude = GetLatitudeCoefficient(image) * GetLatitudeValue(image); //szerokość geograficzna
            var longitude = GetLongitudeCoefficient(image) * GetLongitudeValue(image); //długość geograficzna
            return new GpsData()
            {
                latitude = latitude,
                longitude = longitude,
            };
        }

        public DateTime GetDateTime(Image image)
        {
            const int creationDateIndex = 306;
            const string dateTimeFormat = "yyyy:MM:dd HH:mm:ss";
            var creationDateBytes = image.GetPropertyItem(creationDateIndex).Value;
            var creationDateString = Encoding.ASCII.GetString(creationDateBytes);
            creationDateString = creationDateString.Substring(0, creationDateString.Length - 1);
            DateTime.TryParseExact(creationDateString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var creationDate);

            return creationDate;
        }

        private double GetLatitudeValue(Image image)
        {
            const int latitudeIndex = 2;
            try
            {
                var GpsLatitude = image.GetPropertyItem(latitudeIndex).Value; 
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
