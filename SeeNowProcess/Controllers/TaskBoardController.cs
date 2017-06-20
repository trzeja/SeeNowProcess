﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using SeeNowProcess.Controllers;
using System.Net;

namespace SeeNowProcess.Controllers
{
    public class TaskBoardController : DIContextBaseController
    {

        public ActionResult ChangeCurrentProject(string id)
        {
            if (Session["project"] == null)
            {
                if (id == "")
                {
                    return new JsonResult { Data = "NoChange", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    Session["project"] = id;
                    return new JsonResult { Data = "Change", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //return RedirectToAction("BacklogIndex", "Backlog");
                }
            }
            else if (!Session["project"].Equals(id))
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
                if ((Session["project"] == null) || (Session["project"].Equals("")))
                    return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                else
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
        // GET: TaskBoard
        public ActionResult TaskBoardIndex()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult GetProblems()
        {
            using (db)
            {
                if ((Session["project"] == null) || (Session["project"].Equals("")))
                {
                    return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                int id = Int32.Parse(Session["project"].ToString());
                var resultJ = db.Problems.Where(u => u.Box.Project.ProjectID == id).Select(a => new
                {
                    BoxOrder = a.Box.Order,
                    UserSID = a.Story.UserStoryID,
                    Id = a.ProblemID,
                    Title = a.Title,
                    Description = a.Description
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetBoxes()
        {
            using (db)
            {
                if ((Session["project"] == null) || (Session["project"].Equals("")))
                {
                    return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                int id = Int32.Parse(Session["project"].ToString());
                var resultJ = db.Boxes.Where(u => u.Project.ProjectID == id).Select(a => new
                {
                    BoxOrder = a.Order,
                    Name = a.Name
                }).Distinct();
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetUserStories()
        {
            using (db)
            {
                if ((Session["project"] == null) || (Session["project"].Equals("")))
                {
                    return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                int id = Int32.Parse(Session["project"].ToString());
                var resultJ = db.UserStories.Where(u => u.Project.ProjectID == id).Select(a => new
                {
                    UserSID = a.UserStoryID,
                    Title = a.Title
                });
                var data = resultJ.ToList();
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult UpdateDatabase(int problemID, int newUserStoryID, int newBoxOrder)
        {
            using (db)
            {
                int projectId = int.Parse(Session["project"].ToString());                
                var newBoxId = db.Boxes.Where(b => b.Project.ProjectID == projectId && b.Order == newBoxOrder).FirstOrDefault().BoxID;  
                var problem = db.Problems.Where(p => p.ProblemID == problemID).FirstOrDefault();
                var newBox = db.Boxes.Where(b => b.BoxID == newBoxId).FirstOrDefault();
                var newUserStory = db.UserStories.Where(u => u.UserStoryID == newUserStoryID).FirstOrDefault();                

                if (problem == null)
                    return Json("Error - no such task!", JsonRequestBehavior.AllowGet);
                if (newUserStory == null)
                    return Json("Error - no such story!", JsonRequestBehavior.AllowGet);
                if (newBox == null)
                    return Json("Error - no such box!", JsonRequestBehavior.AllowGet);

                var oldBox = problem.Box;
                
                problem.Box = newBox;
                newBox.Problems.Add(problem);
                oldBox.Problems.Remove(problem);

                db.MarkAsModified(newBox);
                db.MarkAsModified(oldBox);

                var oldUserStory = problem.Story;

                problem.Story = newUserStory;
                newUserStory.Problems.Add(problem);
                oldUserStory.Problems.Remove(problem);

                db.MarkAsModified(problem);
                db.MarkAsModified(newUserStory);
                db.MarkAsModified(oldUserStory);
                                
                db.SaveChanges();
                
                return new JsonResult { Data = newBoxId, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}