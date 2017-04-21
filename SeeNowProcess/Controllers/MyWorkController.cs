using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using SeeNowProcess.Controllers;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class MyWorkController : DIContextBaseController
    {
        public MyWorkController(ISeeNowContext context) : base(context) { }
        
        // GET: MyWork
        public ActionResult Index(int? count)
        {
            using (db)
            {
                var all = db.Users.ToList();
                if (count == null)
                {
                    return View(all);
                }

                return View(all);
            }
        }
    }
}