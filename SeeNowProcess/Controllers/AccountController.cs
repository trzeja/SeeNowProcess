using System;
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
            if (Session["user"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult LoginAction()
        {
            using (db)
            {
                string login = Request.Form["login"];
                string password = Request.Form["password"];
                var user = db.Users.Where(x => x.Login.Equals(login) && x.Password.Equals(password)).FirstOrDefault();
                if (user != null)
                {
                    Session["user"] = user;
                    return new JsonResult { Data = "Success"/*"Username: " + user.Name + " Login: " + user.Login*/, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else {
                    return new JsonResult { Data = "Incorrect data", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    /*
                     ~KK - raczej rozsądniejsze wydaje mi się odrzucenie całego żądania niż zwracanie "błędny login"/"błędne hasło"
                       
                    var user2 = db.Users.Where(x => x.Login.Equals(login)).FirstOrDefault();
                    if (user2 != null)
                    {
                        return Content("Password Error");
                    }
                    return Content("Login Error");*/
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