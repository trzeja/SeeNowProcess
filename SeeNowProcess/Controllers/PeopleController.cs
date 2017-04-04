﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class PeopleController : Controller
    {
        // GET: People
       /* public ActionResult Index()
        {
            return View();
        }
        [HttpPost]*/
        public ActionResult Index(int? count)
        {
            using (var dbContext = new SeeNowContext())
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