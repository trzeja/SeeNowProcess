using SeeNowProcess.Models;
using SeeNowProcess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeeNowProcess.Controllers
{
    public class AddController : DIContextBaseController
    {
        // GET: Add
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexAdd()
        {
            using (db)
            {
                Models.Problem problem = new Models.Problem();
                problem.Title = Request.Form["title"];
                problem.Description = Request.Form["description"];
                db.Problems.Add(problem);
                db.SaveChanges();

                //problem.CurrentState = Request.Form["status"];

                    //db.Problems.Add(problem);
                    //db.SaveChanges();
                    //return RedirectToAction("/MyWork/Index");
            }
            // return View(problem);
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStories()
        {
            using (db)
            {
                var stories = db.UserStories
                    .Select(t => new
                    {
                        UserStory = t.Title
                    });
                //przypisań do projektu chyba nie robimy?
                return new JsonResult
                {
                    Data = new
                    {
                        stories = stories.ToList()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        //
        // POST: /Account/Login
        /*    [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }*/
    }


}