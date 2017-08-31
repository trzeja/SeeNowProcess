using SeeNowProcess.Controllers;
using SeeNowProcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class CreateController : DIContextBaseController
    {
        // GET: Create
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult AddProject([Bind(Include = "Name,Description,Status,StartDate,CompletionDate")] Project project)
        {
            using (db)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
    }
}