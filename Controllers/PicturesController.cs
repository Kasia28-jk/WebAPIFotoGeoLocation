using Microsoft.AspNetCore.Mvc;

namespace FotoGeoLocationWebApplication.Controllers
{
    public class PicturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
