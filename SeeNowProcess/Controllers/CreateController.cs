using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class CreateController : Controller
    {
        // GET: Create
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //Potrzebna metoda POST przyjmująca dane z angulara (przesłanie takie jak wcześniej) MA NAZWAĆ SIĘ ADDPROJECT !!!
        //po kolei: title, description, startdate, enddate (nie wiem czemu to przy tworzeniu ale ok - chyba żeby dodać estimate enddate albo wgl to wyrzucić), status

    }
}