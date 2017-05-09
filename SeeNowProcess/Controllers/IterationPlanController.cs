﻿using SeeNowProcess.Controllers;
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
                             .Where(i => i.Project.ProjectID == project.ProjectID && i.Name.StartsWith("Unnamed"))
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
                //jako że mamy hierarchię problem->box->iteration plus założenie, że każda iteracja ma te same boxy
                //  to znajdę docelowego boxa po nazwie bieżącego
                if(!db.Iterations.Where(i => i.IterationId == newIterationId).Any())
                    return Json("No such iteration!", JsonRequestBehavior.AllowGet);
                Box newBox = db.Boxes.Where(b => b.Iteration.IterationId == newIterationId && b.Name.Equals(problem.Box.Name)).FirstOrDefault();
                if(newBox == null)
                    return Json("Cannot find target box!", JsonRequestBehavior.AllowGet);
                problem.Box = newBox;
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

        [HttpPost]
        public ActionResult GetIteration(int id)
        {
            using (db)
            {
                var problems = db.Problems
                        .Where(p => p.Box.Iteration.IterationId == id)
                        .Select(p => new
                        {
                            title = p.Title,
                            description = p.Description
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
                            id_iteracji = p.Box.Iteration.IterationId
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