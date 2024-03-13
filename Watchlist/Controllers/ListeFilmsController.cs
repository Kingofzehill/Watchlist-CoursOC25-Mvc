using Microsoft.AspNetCore.Mvc;

namespace Watchlist.Controllers
{
    public class ListeFilmsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
