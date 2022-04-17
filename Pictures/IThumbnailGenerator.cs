using System;
using System.Drawing;

namespace FotoGeoLocationWebApplication.Pictures
{
    public interface IThumbnailGenerator
    {
        Byte[] GetThumbnailImageData(Image image, int maxWidth, int maxHeight);
    }
}