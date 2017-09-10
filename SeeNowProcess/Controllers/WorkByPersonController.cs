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
        public ActionResult WorkByPersonIndex()
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
        //here
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
                if (Session["project"].Equals("0"))
                {
                    int id = Int32.Parse(Session["project"].ToString());
                    var resultJ = db.Problems.Select(a => new
                    {
                        BoxOrder = a.Box.Order,
                        UserSID = a.Story.UserStoryID,
                        Id = a.ProblemID,
                        Title = a.Title,
                        Description = a.Description,
                        ProjectID = a.Box.Project.ProjectID,
                        AssignedUsers = a.AssignedUsers.Select(u => u.UserID).ToList()
                    });
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    int id = Int32.Parse(Session["project"].ToString());
                    var resultJ = db.Problems.Where(u => u.Box.Project.ProjectID == id).Select(a => new
                    {
                        BoxOrder = a.Box.Order,
                        UserSID = a.Story.UserStoryID,
                        Id = a.ProblemID,
                        Title = a.Title,
                        Description = a.Description,
                        ProjectID = a.Box.Project.ProjectID,
                        AssignedUsers = a.AssignedUsers.Select(u => u.UserID).ToList()
                    });
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
        }

        public ActionResult GetBoxes()
        {
            using (db)
            {
                if (Session["project"].Equals("0"))
                {
                    var resultJ = db.Boxes.Select(a => new
                    {
                        BoxOrder = a.Order,
                        Name = a.Name
                    }).Distinct();
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    int id = Int32.Parse(Session["project"].ToString());
                    var resultJ = db.Boxes.Where(u => u.Project.ProjectID == id).Select(a => new
                    {
                        BoxOrder = a.Order,
                        Name = a.Name
                    }).Distinct();
                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
        }

        public ActionResult GetUsers()
        {
            
            // tu w ogóle zamieszanie, bo jak taski przesuwamy, to tylko w odrębie projektu danego? 
            //czy jakoś bardziej podzielic, ale bez sensu to to... 
            using (db)
            {
                if (Session["project"].Equals("0"))
                {
                    var resultJ = db.Users.Select(p => new
                    {
                        UserID = p.UserID,
                        Name = p.Name
                    });

                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    int projectId = int.Parse(Session["project"].ToString());
                    var resultJ = db.Users.Select(p => new
                    {
                        UserID = p.UserID,
                        Name = p.Name
                    });

                    return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
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
                        ProjectID = p.Box.Project.ProjectID,
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

        //czy tu tylko przesuwanie miedzy
        [HttpPost]
        public ActionResult UpdateDatabase(int problemID, int oldUserID, int newUserID, int newBoxOrder, int projectId)
        {
            string mess = "";
            using (db)
            {
                //int projectId = int.Parse(Session["project"].ToString());
                var newBoxId = db.Boxes.Where(b => b.Project.ProjectID == projectId && b.Order == newBoxOrder).FirstOrDefault().BoxID;
                var problem = db.Problems.Where(p => p.ProblemID == problemID).FirstOrDefault();
                var newBox = db.Boxes.Where(b => b.BoxID == newBoxId).FirstOrDefault();
                var newUser = db.Users.Where(u => u.UserID == newUserID).FirstOrDefault();
                var oldUser = db.Users.Where(u => u.UserID == oldUserID).FirstOrDefault();

                if (problem == null)
                    mess = "Error - no such task!";
                else if (newUser == null)
                    mess = "Error - no such user!";
                else if (newBox == null)
                    mess = "Error - no such box!";
                else
                {

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
                        return Json(mess, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        mess = e.Message;
                    }
                }
                bool wasError = (mess != "");
                if (!wasError)
                    mess = newBoxId.ToString();


                return Json(new { error = wasError, result = mess }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}