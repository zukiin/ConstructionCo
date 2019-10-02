using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ds3App.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {

    }
}