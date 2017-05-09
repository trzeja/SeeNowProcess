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
        //Create([Bind(Include = "Id,SupervisorId,LastName,FirstName")] Worker worker)
        public ActionResult AddingIteration([Bind(Include = "Name,Description,StartDate,EndDate")] Iteration iteration)
        {
            using (db)
            {
                // chyba teraz nie rozrozniamy projektu
                Project project = db.Projects.FirstOrDefault();
                iteration.Project = project;
                //zabezpieczenie przed duplikacją nazw "Unnamed"
                List<Iteration> unnameds;
                unnameds = db.Iterations
                             .Where(i => i.Project.ProjectID == project.ProjectID && i.Name.StartsWith(iteration.Name))
                             .OrderBy(i => i.Name)
                             .ToList();
                int number = 0;
                if (unnameds.Exists(i => i.Name.Equals(iteration.Name)))
                {
                    for (number = 1; unnameds.Exists(i => i.Name.Equals(iteration.Name + " (" + number + ")")); ++number) ;
                    iteration.Name += " (" + number + ")";
                }
                db.Iterations.Add(iteration);
                db.SaveChanges();
                return new JsonResult { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult MoveTask(int taskId, int newIterationId)
        {
            using (db)
            {
                Problem problem = db.Problems.Where(p => p.ProblemID == taskId).FirstOrDefault();
                if(problem == null)
                    return Json("No such task!", JsonRequestBehavior.AllowGet);

                var newIteration = db.Iterations.Where(i => i.IterationId == newIterationId).FirstOrDefault();
                //jako że mamy hierarchię problem->box->iteration plus założenie, że każda iteracja ma te same boxy
                //  to znajdę docelowego boxa po nazwie bieżącego
                if(newIteration == null)
                    return Json("No such iteration!", JsonRequestBehavior.AllowGet);
                problem.Iteration = newIteration;
                db.MarkAsModified(problem);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
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
                        id = i.IterationId,
                        name = i.Name,
                        duration = (i.StartDate == null ? "?" : i.StartDate.ToString()) + " - " + (i.EndDate == null ? "?" : i.EndDate.ToString())
                    });
                return Json(iterations.ToList(), JsonRequestBehavior.AllowGet);

            }
                            /*tutaj prosze o zwrocenie listy obiektow skladajacych sie z: 
                                {name: "nazwa iteracji", duration: "czas trwania(od kiedy do kiedy)"} 
                            */
        }

        [HttpGet]
        public ActionResult GetAllIterations()
        {
            using (db)
            {
                var problems = db.Problems
                        .Select(p => new
                        {
                            id = p.ProblemID,
                            title = p.Title,
                            description = p.Description,
                            id_iteracji = p.Iteration.IterationId
                            // jak cos wiecej potrzebne to dopisujcie tutaj, byle typy proste, nie obiekty i inne cuda
                        });
                return Json(problems.ToList(), JsonRequestBehavior.AllowGet);
                /* return Json(
                     db.Problems
                         .Where(p => p.Box.Iteration.IterationId == id)
                     , JsonRequestBehavior.AllowGet);*/

            }
            /* tutaj zwracamy taski z konkretnej iteracji (cala ich liste) - iteracja podana jako parametr (id) - 
                        z wszystkich boxow same taski poprosze bo inaczej to sie widok zakopie totalnie...*/
        }

    }

}