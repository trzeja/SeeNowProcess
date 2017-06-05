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
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                            Title = p.Title,
                            Description = p.Description
                            //wysylac jeszcze inne informacje o tasku potem, np. order
                        });
                return Json(tasks.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}