﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;

namespace SeeNowProcess.Controllers
{
    public class AccountController : DIContextBaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAction()
        {
            using (db)
            {
                string login = Request.Form["login"];
                string password = Request.Form["password"];
                /*var user = db.Users.Where(x => x.Login.Equals(login) && x.Password.Equals(password)).FirstOrDefault();
                if (user != null)
                {
                    Session["user"] = user;
                    return new JsonResult { Data = "Username: " + user.Name + " Login: " + user.Login, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else {
                    var user2 = db.Users.Where(x => x.Login.Equals(login)).FirstOrDefault();
                    if (user2 != null)
                    {
                        return Content("Password Error");
                    }
                    return Content("Login Error");
                }*/
                var test = db.Users.Where(X => X.Login.Equals(login));
                var user = db.Users.Where(X => X.Login.Equals(login)).FirstOrDefault();
                if (user == null)
                {
                    return Content("Login Error");
                } else if (user.ComparePassword(password))
                {
                    Session["user"] = user;
                    return new JsonResult { Data = "Username: " + user.Name + " Login: " + user.Login, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                } else
                {
                    return Content("Password Error");
                }
               // return Content("Login or password uncorrect");
            }
            // return Content("Login: " + Request.Form["login"] + "Paswword: " + Request.Form["password"]);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAction()
        {
            return View();
        }


    }
}