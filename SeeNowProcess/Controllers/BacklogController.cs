using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class BacklogController : Controller
    {
        // GET: Backlog
        public ActionResult Index()
        {
            return View();
        }
    }
}