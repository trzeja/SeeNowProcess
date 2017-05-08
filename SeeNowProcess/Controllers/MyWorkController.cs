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



        public ActionResult IndexJS()
        {
            using (db)
            {
                var resultJ = db.Problems.Select(a => new
                {
                    ProblemID = a.ProblemID,
                    Title = a.Title,
                    Description = a.Description
                    /*CurrentState = a.CurrentState*/
                });
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
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

                Problem movedProblem = db.Problems.Find(int.Parse(ProblemID));

                if (movedProblem == null)
                {
                    return HttpNotFound();
                }

                Box oldBox = movedProblem.Box;

                //Iteration iteration = oldBox.Iteration;

                //var iterationBoxes = iteration.Boxes.Where(b => b.Order == int.Parse(NewState));

                //Box newBox = iterationBoxes.FirstOrDefault();

                //db.Problems.Find(movedProblem.ProblemID).Box == newBox;

                //db.Problems.Remove(movedProblem);

                //movedProblem.Box = newBox;

                //movedProblem.Box = db.Boxes.Where(b => b.Order == newOrder).FirstOrDefault();

                //db.MarkAsModified<Problem>(movedProblem);

                //trzeba kasowac i dodawac uprzednio tworzac kopie?? moze, inaczej nie widze jak                


                //tak sie nie da
                //oldBox.Problems.Delete(movedProblem);
                //db.MarkAsModified<Box>(oldBox);

                //newBox.Problems.Add(movedProblem);
                //db.MarkAsModified<Box>(oldBox);

                db.SaveChanges();

                return View();
            }
        }

    }
}