﻿using SeeNowProcess.Controllers;
using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    public class IterationPlanController : DIContextBaseController
    {
        // GET: IterationPlan
        public ActionResult IterationPlanIndex()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            using (db)
            {
                if (Session["project"] == null)
                {
                    Session["project"] = "0";
                }
                var projects = db.Projects;
                return View(projects.ToList());
            }
        }

        public ActionResult ChangeCurrentProject(string id)
        {
            if (!Session["project"].Equals(id))
            {
                Session["project"] = id;
                return new JsonResult { Data = "Change", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //return RedirectToAction("BacklogIndex", "Backlog");
            }
            return new JsonResult { Data = "NoChange", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetCurrentProject()
        {
            using (db)
            {
                return new JsonResult { Data = Session["project"], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetCurrentProjectName()
        {
            using (db)
            {
                if (Session["project"].Equals("0"))
                {
                    return new JsonResult { Data = "All projects", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    int idProject = Int32.Parse(Session["project"].ToString());
                    Project project = db.Projects.Where(p => p.ProjectID == idProject).FirstOrDefault();
                    return new JsonResult { Data = project.Name, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                
            }
        }



        [HttpPost]
        //Create([Bind(Include = "Id,SupervisorId,LastName,FirstName")] Worker worker)
        public ActionResult AddingIteration([Bind(Include = "Name,Description,StartDate,EndDate")] Iteration iteration, string idProject)
        {
            using (db)
            {
                //tu bedzie dodawany z formatki nie? - R.
                // chyba teraz nie rozrozniamy projektu
                int idPr = Int32.Parse(idProject.ToString());
                Project project = db.Projects.Where(p => p.ProjectID == idPr).FirstOrDefault();
                iteration.Project = project;
                //Project project = db.Projects.FirstOrDefault();
                //iteration.Project = project;
                
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

        //Czy można taski między iteracjami z różnych projektów?
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

        [HttpPost]
        public ActionResult GetProjectForIteration(string iterationId)
        {
            using (db) {
                int id = Int32.Parse(iterationId.ToString());
                Iteration iteration = db.Iterations.Where(i => i.IterationId == id).FirstOrDefault();
                Project project = db.Projects.Where(p => p.ProjectID == iteration.Project.ProjectID).FirstOrDefault();
                List<string> list = new List<string>();
                list.Add(project.ProjectID.ToString());
                list.Add(project.Name);
                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult GetNumber()
        {
            using (db)
            {
                if (Session["project"].Equals("0"))
                {
                    var iterations = db.Iterations
                    .Select(i => new
                    {
                        id = i.IterationId,
                        name = i.Name,
                        projectName = i.Project.Name,
                        duration = (i.StartDate == null ? "?" : i.StartDate.ToString()) + " - " + (i.EndDate == null ? "?" : i.EndDate.ToString())
                    });
                    return Json(iterations.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int idProject = Int32.Parse(Session["project"].ToString());
                    //Project project = db.Projects.Where(p => p.ProjectID == idProject).FirstOrDefault();
                    var iterations = db.Iterations
                        .Where(i => i.Project.ProjectID == idProject)
                        .Select(i => new
                        {
                            id = i.IterationId,
                            name = i.Name,
                            projectName = i.Project.Name,
                            duration = (i.StartDate == null ? "?" : i.StartDate.ToString()) + " - " + (i.EndDate == null ? "?" : i.EndDate.ToString())
                        });
                    return Json(iterations.ToList(), JsonRequestBehavior.AllowGet);
                }

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
                if (Session["project"].Equals("0"))
                {
                    var problems = db.Problems
                        .Select(p => new
                        {
                            id = p.ProblemID,
                            title = p.Title,
                            description = p.Description,
                            id_iteracji = p.Iteration == null ? 1 : p.Iteration.IterationId
                            // jak cos wiecej potrzebne to dopisujcie tutaj, byle typy proste, nie obiekty i inne cuda
                        });
                    return Json(problems.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int idProject = Int32.Parse(Session["project"].ToString());
                    var problems = db.Problems.Where(p => p.Box.Project.ProjectID == idProject)
                        .Select(p => new
                        {
                            id = p.ProblemID,
                            title = p.Title,
                            description = p.Description,
                            id_iteracji = p.Iteration == null ? 1 : p.Iteration.IterationId
                            // jak cos wiecej potrzebne to dopisujcie tutaj, byle typy proste, nie obiekty i inne cuda
                        });
                    return Json(problems.ToList(), JsonRequestBehavior.AllowGet);
                }


                //var problems = db.Problems
                //        .Select(p => new
                //        {
                //            id = p.ProblemID,
                //            title = p.Title,
                //            description = p.Description,
                //            id_iteracji = p.Iteration==null ? 1 : p.Iteration.IterationId
                //            // jak cos wiecej potrzebne to dopisujcie tutaj, byle typy proste, nie obiekty i inne cuda
                //        });
                //return Json(problems.ToList(), JsonRequestBehavior.AllowGet);
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