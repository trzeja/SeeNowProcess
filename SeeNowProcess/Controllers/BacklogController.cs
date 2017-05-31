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

        public ActionResult GetUserStories()
        {
            using (db)
            {
                var resultJ = db.UserStories.Select(a => new
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