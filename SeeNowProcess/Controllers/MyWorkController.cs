﻿using System;
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
                var resultJ = db.Problems.Select(a => new {
                ProblemID = a.ProblemID,
                Title = a.Title,
                Description = a.Description
                /*CurrentState = a.CurrentState*/});
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

                //if (string.IsNullOrEmpty(NewState)) //null gdy wlozony do tego samego boxa, ale musza byc inne w tym boxie
                //{
                //    return View(); // nic wtedy nie robimy
                //}

                //if (string.IsNullOrEmpty(ProblemID))
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}
                
                //Problem transferredProblem = db.Problems.Find(int.Parse(ProblemID));

                //if (transferredProblem == null)
                //{
                //    return HttpNotFound();
                //}

                //Box newBox = db.Boxes.Find(int.Parse(NewState));

                //Box oldBox = transferredProblem.Box;

                //Iteration iteration = oldBox.Iteration;

                //iteration

                return View();
            }
        }

    }
}