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
    public class MyWorkController : DIContextBaseController
    {
        // public MyWorkController(ISeeNowContext context) : base(context) { }


        //public ActionResult GetCurrentUser()
        //{
        //    //string user = Session.GetDataFromSession<string>("user");
        //    //ViewData["sessionStringByExtensions"] = user;
        //    var user = Session["user"];
        //    return new JsonResult { Data = user.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        // GET: MyWork
        public ActionResult MyWorkIndex(int? count)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            using (db)
            { 
                var all = db.Problems.ToList();
                if (count == null)
                {
                    return View(all);
                }

                return View(all);
            }
        }

        public ActionResult GetBoxOrder()
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

        public ActionResult IndexJS()
        {
            using (db)
            {
                int id = Int32.Parse(Session["user"].ToString());
                var resultJ = db.Problems.Where(p => p.AssignedUsers.Select(u=>u.UserID).Contains(id)).Select(a => new
                {
                    ProblemID = a.ProblemID,
                    Title = a.Title,
                    Description = a.Description,
                    BoxOrder = a.Box.Order
                    /*CurrentState = a.CurrentState*/
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult UpdateDatabase(int ProblemID, int NewState)
        {
            using (db)
            {
                var problem = db.Problems.Where(p => p.ProblemID == ProblemID).FirstOrDefault();
                var newBox = db.Boxes.Where(b => b.Order == NewState).FirstOrDefault();
                if(problem == null)
                    return Json("Error - no such task!", JsonRequestBehavior.AllowGet);
                if(newBox == null)
                    return Json("Error - no such box!", JsonRequestBehavior.AllowGet);
                problem.Box = newBox;
                db.MarkAsModified(problem);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
    }
}