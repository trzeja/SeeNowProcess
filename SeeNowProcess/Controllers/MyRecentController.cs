using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class MyRecentController : Controller
    {
        // GET: MyRecent
        public ActionResult Index()
        {
            //if (Session["user"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            return View();
        }
    }
}