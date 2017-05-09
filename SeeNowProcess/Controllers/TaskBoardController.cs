using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    public class TaskBoardController : DIContextBaseController
    {
        // GET: TaskBoard
        public ActionResult TaskBoardIndex()
        {
            return View();
        }

        public ActionResult GetBoxes()
        {
            using (db)
            {
                var resultJ = db.Boxes.Select(a => new
                {
                    BoxOrder = a.Order,
                    Name = a.Name
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
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
    }
}