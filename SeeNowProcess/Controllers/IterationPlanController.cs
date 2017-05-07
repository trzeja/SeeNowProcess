using SeeNowProcess.Controllers;
using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class IterationPlanController : DIContextBaseController
    {
        // GET: IterationPlan
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddingIteration()
        {
            using (db)
            {
                // chyba teraz nie rozrozniamy projektu
                Project project = db.Projects.FirstOrDefault();
                Iteration iteration = new Iteration() {Description="", Name="Unnamed", Project=project};
                //zabezpieczenie przed duplikacją nazw "Unnamed"
                List<Iteration> unnameds;
                unnameds = db.Iterations
                             .Where(i => i.Project == project && i.Name.StartsWith("Unnamed"))
                             .OrderBy(i => i.Name)
                             .ToList();
                int number = 0;
                if (unnameds.Exists(i => i.Name.Equals(iteration.Name)))
                {
                    for (number = 1; unnameds.Exists(i => i.Name.Equals(iteration.Name + " (" + number + ")")); ++number) ;
                    iteration.Name += " (" + number + ")";
                }
                db.Iterations.Add(iteration);
                return new JsonResult { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult GetNumber()
        {
            using (db)
            {

                // chyba teraz nie rozrozniamy projektu
                Project project = db.Projects.FirstOrDefault();
                var iterations = db.Iterations
                    .Where(i => i.Project.ProjectID == project.ProjectID)
                    .Select(i => new
                    {
                        id=i.IterationId,
                        name = i.Name,
                        duration = (i.StartDate == null ? "?" : i.StartDate.ToString()) + " - " + (i.EndDate == null ? "?" : i.EndDate.ToString())
                    });
                return Json(iterations.ToList(), JsonRequestBehavior.AllowGet);

            }
                            /*tutaj prosze o zwrocenie listy obiektow skladajacych sie z: 
                                {name: "nazwa iteracji", duration: "czas trwania(od kiedy do kiedy)"} 
                            */
        }

        [HttpPost]
        public ActionResult GetIteration(int id)
        {
            using (db)
            {
                return Json(
                    db.Problems
                        .Where(p => p.Box.Iteration.IterationId == id)
                    , JsonRequestBehavior.AllowGet);
                    
            }
                /* tutaj zwracamy taski z konkretnej iteracji (cala ich liste) - iteracja podana jako parametr (id) - 
                            z wszystkich boxow same taski poprosze bo inaczej to sie widok zakopie totalnie...*/
        }

    }

}