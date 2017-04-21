using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class MyWorkController : Controller
    {
        private ISeeNowContext dbContext = new SeeNowContext();

        public MyWorkController() { }

        public MyWorkController(ISeeNowContext context)
        {
            dbContext = context;
        }
        // GET: MyWork
        public ActionResult Index(int? count)
        {
            using (dbContext)
            {
                var all = dbContext.Users.ToList();
                if (count == null)
                {
                    return View(all);
                }

                return View(all);
            }
        }
    }
}