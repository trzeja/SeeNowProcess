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
        public ActionResult Index(int? count)
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
                Description = a.Description});
                //var all = db.Problems.ToList();
                //JsonResult result = new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //return new JsonResult {Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return new JsonResult { Data = resultJ.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}