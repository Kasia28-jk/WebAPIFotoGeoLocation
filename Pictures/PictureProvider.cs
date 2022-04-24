using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Entities;
using FotoGeoLocationWebApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FotoGeoLocationWebApplication.Pictures
{
    public class PictureProvider : IPictureProvider
    {
        private readonly DataContext _dataContext;

        public PictureProvider(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<PictureDto> GetPictures(int userId)
        {
            var pictures =  _dataContext.Pictures
                .Where(x => x.UserId.Equals(userId))
                .Select(x => CreatePictureDto(x)).ToList();
            return pictures;
            
        }

        public IEnumerable<PictureDto> GetPicturesOfEveryUser()
        {
            var pictures = _dataContext.Pictures.Select(x => CreatePictureDto(x)).ToList();
            return pictures;
        }

        private static PictureDto CreatePictureDto(Picture x)
        {
            byte[] bytes = File.ReadAllBytes(x.Path);
            string image = Convert.ToBase64String(bytes);

            return new PictureDto()
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                File = image,
                UserName = x.UserName,
            };
        }
    }
}
