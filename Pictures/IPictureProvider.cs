using FotoGeoLocationWebApplication.Models;
using System.Collections.Generic;

namespace FotoGeoLocationWebApplication.Pictures
{
    public interface IPictureProvider
    {
        IEnumerable<PictureDto> GetPictures(int userId);
    }
}