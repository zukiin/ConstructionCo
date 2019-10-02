using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ds3App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Home()
        {
            if (User.Identity.IsAuthenticated) return View("Index");
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [Route("bad-request")]
        [AllowAnonymous]
        public ActionResult BadRequest()
        {
            return View();
        }
        
    }
}