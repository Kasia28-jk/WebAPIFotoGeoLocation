using FotoGeoLocationWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace FotoGeoLocationWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadPicturesController : Controller
    {
        private readonly IWebHostEnvironment _env;
     //   private readonly IHttpContextAccessor _contextAccessor;

        public UploadPicturesController(/*IHttpContextAccessor contextAccessor,*/ IWebHostEnvironment env)//IWebHostEnvironment env)
        {
          //  _contextAccessor = contextAccessor;
            _env = env;
        }

        [HttpPost, DisableRequestSizeLimit]
        public string Post(IFormFile file)
        {
            // var context = _contextAccessor.HttpContext;
            //var file = HttpContext.Request.Form.Files.Count > 0 ? HttpContext.Request.Form.Files[0] : null;

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var guid = Guid.NewGuid();
                var path = Path.Combine(_env.ContentRootPath, "UploadedPictures", $"{guid}{fileName}");
                var f = new FileStream(path, FileMode.Create);
                file.CopyTo(f);
                f.Flush();
                f.Close();
                f.Dispose();
            }
            return "test";

            /* if (file != null && file.Length > 0)
             {
                 var fileName = Path.GetFileName(file.FileName);
                 var guid = Guid.NewGuid();
                 var path = Path.Combine(_env.ContentRootPath, "UploadedPictures", $"{guid}{fileName}");
                 var f = new FileStream(path, FileMode.Create);
                 file.CopyTo(f);
                 f.Flush();
                 f.Close();
                 f.Dispose();
             }
             return "test";*/
            //return "success" + file != null ? "/Files/" + file.Name : null;
        }

        [HttpGet]
        public string get()
        {
            return "Hello world !";
        }
    }
}
