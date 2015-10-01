using System.Configuration;
using System.Web.Mvc;
using JeroenPot.Common;

namespace JeroenPot.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}