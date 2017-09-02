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
            if ((Session["project"] == null) || (Session["project"].Equals("")))
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            int projectId = int.Parse(Session["project"].ToString());
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
        public ActionResult UpdateDatabase(int problemID, int oldUserID, int newUserID, int newBoxOrder)
        {
            using (db)
            {
                int projectId = int.Parse(Session["project"].ToString());
                var newBoxId = db.Boxes.Where(b => b.Project.ProjectID == projectId && b.Order == newBoxOrder).FirstOrDefault().BoxID;
                var problem = db.Problems.Where(p => p.ProblemID == problemID).FirstOrDefault();
                var newBox = db.Boxes.Where(b => b.BoxID == newBoxId).FirstOrDefault();
                var newUser = db.Users.Where(u => u.UserID == newUserID).FirstOrDefault();
                var oldUser = db.Users.Where(u => u.UserID == oldUserID).FirstOrDefault();

                if (problem == null)
                    return Json("Error - no such task!", JsonRequestBehavior.AllowGet);
                if (newUser == null)
                    return Json("Error - no such user!", JsonRequestBehavior.AllowGet);
                if (newBox == null)
                    return Json("Error - no such box!", JsonRequestBehavior.AllowGet);

                var oldBox = problem.Box;

                problem.Box = newBox;
                newBox.Problems.Add(problem);
                
                db.MarkAsModified(newBox);
                db.MarkAsModified(oldBox);

                problem.AssignedUsers.Add(newUser);
                newUser.Problems.Add(problem);
                oldUser.Problems.Remove(problem);                

                db.MarkAsModified(problem);
                db.MarkAsModified(newUser);
                db.MarkAsModified(oldUser);

                db.SaveChanges();

                return new JsonResult { Data = newBoxId, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }

}