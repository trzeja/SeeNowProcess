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
                var resultJ = db.Problems.Select(a => new
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
        /*
        public ActionResult UpdateDatabase()
        {
            using (db)
            {
                //trzeba wyszukać w bazie taska po id, wywalic go z obecnego boxa, przypisac do nowego boxa
                var ProblemID = Request.Form["ProblemID"];
                var NewState = Request.Form["NewState"]; //numer albo "null"                              

                if (string.IsNullOrEmpty(NewState)) //null gdy wlozony do tego samego boxa, ale musza byc inne w tym boxie
                {
                    return View(); // nic wtedy nie robimy
                }

                int newOrder = int.Parse(NewState); // variable representing evil in galaxy

                if (string.IsNullOrEmpty(ProblemID))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                int problemId = int.Parse(ProblemID);

                Problem movedProblem = db.Problems.Find(problemId);

                if (movedProblem == null)
                {
                    return HttpNotFound();
                }

                Box oldBox = movedProblem.Box;

                oldBox.Problems.Remove(movedProblem);
                db.MarkAsModified(oldBox);                

                Iteration iteration = oldBox.Iteration;
                Box newBox = iteration.Boxes.Where(b => b.Order == newOrder).FirstOrDefault();

                movedProblem.Box = newBox;
                db.MarkAsModified(movedProblem);
                
                newBox.Problems.Add(movedProblem);
                db.MarkAsModified(newBox);
                
                db.SaveChanges();
                return View();
            }
        }
        */

    }
}