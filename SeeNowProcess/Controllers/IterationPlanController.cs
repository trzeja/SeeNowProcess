using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class IterationPlanController : Controller
    {
        // GET: IterationPlan
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddingIteration()
        {
            return View();  /* tutaj dodawanie iteracji do bazy poprosze
                                
                            */
        }

        [HttpGet]
        public ActionResult GetNumber()
        {
            return View();  /*tutaj prosze o zwrocenie listy obiektow skladajacych sie z: 
                                {name: "nazwa iteracji", duration: "czas trwania(od kiedy do kiedy)"} 
                            */
        }

        [HttpPost]
        public ActionResult GetIteration()
        {
            //Request.Form["name"];
            return View();  /* tutaj zwracamy taski z konkretnej iteracji (cala ich liste) - iteracja podana jako parametr (id) - 
                            z wszystkich boxow same taski poprosze bo inaczej to sie widok zakopie totalnie...*/
        }

    }

}