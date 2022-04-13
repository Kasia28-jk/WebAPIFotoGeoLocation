using FotoGeoLocationWebApplication.Data;
using FotoGeoLocationWebApplication.Entities;
using FotoGeoLocationWebApplication.Models;
using Microsoft.AspNetCore.Http;
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

        public IEnumerable<PictureDto> GetPictures(string token)
        {
            var session = _dataContext.Sessions.SingleOrDefault(x => x.Token.Equals(token)
                                                                   && x.ExpiresAt > DateTime.UtcNow);
            if(session == null)
            {
                return Enumerable.Empty<PictureDto>();
            }

            var pictures =  _dataContext.Pictures
                .Where(x => x.UserId.Equals(session.UserId))
                .Select(x => CreatePictureDto(x)).ToList();
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
                File = image
            };
        }
    }
}
