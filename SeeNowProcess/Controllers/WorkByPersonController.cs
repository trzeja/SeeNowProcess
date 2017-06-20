using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    public class WorkByPersonController : DIContextBaseController
    {
        // GET: WorkByPerson
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
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
                    Description = a.Description,
                    AssignedUsers = a.AssignedUsers.Select(u => u.UserID).ToList()
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

        public ActionResult GetUsers()
        {
            using (db)
            {
                var resultJ = db.Users.Select(p => new
                {
                    UserID = p.UserID,
                    Name = p.Name
                });

                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [HttpPost]
        public ActionResult GetTasks(int userId)
        {
            using (db)
            {
                var resultJ = db.Problems
                    .Where(p => p.AssignedUsers.Select(u => u.UserID).Contains(userId))
                    .Select(p => new
                    {
                        ProblemID = p.ProblemID,
                        Title = p.Title,
                        Description = p.Description,
                        BoxOrder = p.Box.Order,
                        AssignedUsers = p.AssignedUsers.Select(u => u.UserID).ToList()
                    });
                return new JsonResult
                {
                    Data = new
                    {
                        UserID = userId,
                        Tasks = resultJ.ToList()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
       

        [HttpPost]
        public ActionResult UpdateDatabase(int problemID, int newUserStoryID, int newBoxOrder)
        {
            using (db)
            {
                //                           TO DO
                // czy nie powinnam też oprocz boxorder przesylac boxid? - R.
                // zwrocic Roksanie id boxa do ktorego przeniesiono taska?

                var problem = db.Problems.Where(p => p.ProblemID == problemID).FirstOrDefault();
                var newUserStory = db.UserStories.Where(u => u.UserStoryID == newUserStoryID).FirstOrDefault();
                var newBox = newUserStory.Project.Boxes.Where(b => b.Order == newBoxOrder).FirstOrDefault();
                //var oneBoxFromBoxesFromNewUserStory = newUserStory.Problems.Where(p => p.ProblemID == problemID).FirstOrDefault().Box
                //var boxes = newUserStory.Problems.GroupBy(p=>p.Box.Order == newBoxOrder).




                if (problem == null)
                    return Json("Error - no such task!", JsonRequestBehavior.AllowGet);
                if (newUserStory == null)
                    return Json("Error - no such story!", JsonRequestBehavior.AllowGet);
                if (newBox == null)
                    return Json("Error - no such box!", JsonRequestBehavior.AllowGet);

                var oldBox = problem.Box;

                problem.Box = newBox;
                newBox.Problems.Add(problem);
                db.MarkAsModified(problem);

                //oldBox.Problems.Remove(problem); // moze nie trza


                //db.MarkAsModified(newBox);
                //db.MarkAsModified(oldBox);
                //db.MarkAsModified(newUserStory);
                db.SaveChanges();

                return new JsonResult { Data = newBox.BoxID, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }

}