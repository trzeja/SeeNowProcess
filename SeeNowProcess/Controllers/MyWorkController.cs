using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using SeeNowProcess.Controllers;

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
                var resultJ = db.Problems.Select(a => new {
                ProblemID = a.ProblemID,
                Title = a.Title,
                Description = a.Description,
                CurrentState = a.CurrentState});
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult UpdateDatabase()
        {
            using (db)
            {
                //trzeba wyszukać w bazie id taska i zmienić Current State
                var ProblemID = Request.Form["ProblemID"]; 
                var NewState = Request.Form["NewState"]; //numer albo "null"
                return View();
            }
        }

    }
}