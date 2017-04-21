using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;

namespace SeeNowProcess.Controllers
{
    public class PeopleController : DIContextBaseController
    {
        // GET: People
        /* public ActionResult Index()
         {
             return View();
         }
         [HttpPost]*/
        public PeopleController(ISeeNowContext context):base(context) {}

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