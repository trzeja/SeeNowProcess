﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt_programistyczny_pierwsze_kroki.Controllers
{
    public class TaskBoardController : Controller
    {
        // GET: TaskBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}