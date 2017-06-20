using System;
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
                var data = resultJ.ToList();
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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