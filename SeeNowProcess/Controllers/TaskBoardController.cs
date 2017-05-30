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

        public ActionResult GetProblems()
        {
            using (db)
            {
                var resultJ = db.Problems.Select(a => new
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
                var resultJ = db.Boxes.Select(a => new
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
                var resultJ = db.UserStories.Select(a => new
                {
                    UserSID = a.UserStoryID,
                    Title = a.Title
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult UpdateDatabase(int ProblemID, int UserStoryID, int BoxOrder)
        {
            using (db)
            {
                //                           TO DO
                // czy nie powinnam też oprocz boxorder przesylac boxid? - R.

                //var problem = db.Problems.Where(p => p.ProblemID == ProblemID).FirstOrDefault();
                //var newBox = db.Boxes.Where(b => b.Order == NewState).FirstOrDefault();
                //if (problem == null)
                //    return Json("Error - no such task!", JsonRequestBehavior.AllowGet);
                //if (newBox == null)
                //    return Json("Error - no such box!", JsonRequestBehavior.AllowGet);
                //problem.Box = newBox;
                //db.MarkAsModified(problem);
                //db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
    }
}