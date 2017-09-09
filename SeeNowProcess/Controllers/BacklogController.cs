﻿using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    public class BacklogController : DIContextBaseController
    {
        // GET: Backlog
        public ActionResult BacklogIndex()
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
            //if (Session["project"] == null)
            //{
            //    if (id == "")
            //    {
            //        return new JsonResult { Data = "NoChange", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //    }
            //    else
            //    {
            //        Session["project"] = id;
            //        return new JsonResult { Data = "Change", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //        //return RedirectToAction("BacklogIndex", "Backlog");
            //    }
            //}
            //else if (!Session["project"].Equals(id))
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
                //if ((Session["project"] == null) || (Session["project"].Equals("")))
                //    return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //else
                    return new JsonResult { Data = Session["project"], JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetProjects()
        {
            using (db)
            {
                var resultJ = db.Projects.Select(a => new
                {
                    id = a.ProjectID,
                    name = a.Name
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetUserStories()
        {
            using (db)
            {
                //if ((Session["project"] == null) || (Session["project"].Equals("")))
                if (Session["project"].Equals("0"))
                {
                    var resultJ = db.UserStories.Select(a => new
                    {
                        UserSID = a.UserStoryID,
                        Title = a.Title
                    });
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    int id = Int32.Parse(Session["project"].ToString());
                    var resultJ = db.UserStories.Where(u => u.Project.ProjectID == id).Select(a => new
                    {
                        UserSID = a.UserStoryID,
                        Title = a.Title
                    });
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
        }

        [HttpPost]
        public ActionResult GetUserStory(int id)
        {
            using (db)
            {
                var userstory = db.UserStories.Where(u => u.UserStoryID == id)
                        .Select(p => new
                        {
                            Description = p.Description,
                            Size = p.Size,
                            Unit = p.Unit,
                            Notes = p.Notes,
                            Criteria = p.Criteria,
                            Owner = p.Owner.Name,
                            Project = p.Project.Name
                        });
                return Json(userstory.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetUSTasks(int id)
        {
            using (db)
            {
                var tasks = db.Problems.Where(u => u.Story.UserStoryID == id)
                        .Select(p => new
                        {
                            TaskId = p.ProblemID,
                            Title = p.Title,
                            Description = p.Description,
                            Importance = p.Importance
                            //wysylac jeszcze inne informacje o tasku potem, np. order
                        });
                return Json(tasks.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTask(int taskId)
        {
            using (db)
            {
                var problemList = db.Problems.Where(p => p.ProblemID == taskId);
                if (!problemList.Any())
                    return Json("Error - no such task", JsonRequestBehavior.AllowGet);
                Problem problem = problemList.First();
                db.MarkAsModified(problem.Story);
                db.MarkAsModified(problem.Box);
                db.MarkAsModified(problem.Iteration);
                db.Problems.Remove(problem);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTeams()
        {
            using (db)
            {
                return Json(db.Teams.Select(t => new { ID = t.TeamID, name = t.Name }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddUserStory([Bind(Include ="Title,Description,Size,Unit,Notes,Criteria")] UserStory userStory, int project, List<int>teams, int owner)
        {
            if (teams == null)
                teams = new List<int>();
            using (db)
            {
                string mess = "success";
                List<Team> dbTeams = db.Teams.Where(t => teams.Contains(t.TeamID)).ToList();
                User ownerUser = db.Users.Where(u => u.UserID == owner).FirstOrDefault();
                Project projectUS = db.Projects.Where(p => p.ProjectID == project).FirstOrDefault();
                userStory.Teams = dbTeams;
                userStory.Owner = ownerUser;
                userStory.Project = projectUS;
                db.UserStories.Add(userStory);
                dbTeams.ForEach(team => db.MarkAsModified(team));
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    mess = "Errors:";
                    foreach (var validateError in e.EntityValidationErrors)
                    {
                        mess += "\n\tIn " + validateError.Entry.Entity.GetType().ToString() + ":";
                        foreach (var error in validateError.ValidationErrors)
                        {
                            mess += "\n\t\t" + error.ErrorMessage;
                        }
                    }
                }
                catch (Exception e)
                {
                    mess = e.Message;
                }
                return Json(mess, JsonRequestBehavior.AllowGet);
            }
        }
    }
}