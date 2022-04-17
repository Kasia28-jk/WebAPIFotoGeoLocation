using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FotoGeoLocationWebApplication.Pictures
{
    public class ThumbnailGenerator : IThumbnailGenerator
    {
        public Byte[] GetThumbnailImageData(Image image, int maxWidth, int maxHeight)
        {
            int newWidth;
            int newHeight;
            const long compressionLevel = 80L;
            const int orientationPropertyId = 274;

            var orientationValue = image.GetPropertyItem(orientationPropertyId).Value[0];
            var rotateFlipType = GetRotateFlipType(orientationValue);
            image.RotateFlip(rotateFlipType);

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                double ratioX = (double)maxWidth / image.Width;
                double ratioY = (double)maxHeight / image.Height;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);
            }
            else
            {
                newWidth = image.Width;
                newHeight = image.Height;
            }

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            using (var ms = new MemoryStream())
            {
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, compressionLevel);

                newImage.Save(ms, GetEncoderInfo("image/jpeg"), encoderParameters);

                return ms.ToArray();
            }
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(x => x.MimeType.ToLower().Equals(mimeType.ToLower()));
        }

        private RotateFlipType GetRotateFlipType(int rotateValue)
        {
            return rotateValue switch
            {
                1 => RotateFlipType.RotateNoneFlipNone,
                2 => RotateFlipType.RotateNoneFlipX,
                3 => RotateFlipType.Rotate180FlipNone,
                4 => RotateFlipType.Rotate180FlipX,
                5 => RotateFlipType.Rotate90FlipX,
                6 => RotateFlipType.Rotate90FlipNone,
                7 => RotateFlipType.Rotate270FlipX,
                8 => RotateFlipType.Rotate270FlipNone,
                _ => RotateFlipType.RotateNoneFlipNone,
            };
        }
    }
}
