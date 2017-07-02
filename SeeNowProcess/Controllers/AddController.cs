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
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexAdd([Bind(Include = "Title,Description,Status,Importance,EstimatedTime")] Problem problem, String userStory, int? userStoryId, List<int> users)
        {
            using (db)
            {
                if (!(userStory == null ^ userStoryId == null)) // XNOR - musi być wypełnione dokładnie jedno
                    return Json("Error - use exactly one of [UserStory, UserStoryId]", JsonRequestBehavior.AllowGet);
                //Problem parentProblem = db.Problems.Where(p => p.ProblemID == parentProblemId).FirstOrDefault();
                UserStory story;
                if (userStory == null)
                    story = db.UserStories.Where(us => us.UserStoryID == userStoryId).FirstOrDefault();
                else
                    story = db.UserStories.Where(us => us.Title.Equals(userStory)).FirstOrDefault();
                if (story == null)
                    return Json("Error - cannot find user story", JsonRequestBehavior.AllowGet);
                problem.Story = story;
                problem.Box = db.Boxes.Where(box => box.Project.ProjectID == story.Project.ProjectID).OrderBy(box => box.Order).First();
                List<User> assignedUsers = db.Users.Where(u => users.Contains(u.UserID)).ToList();
                if (assignedUsers.Count < users.Count)
                {
                    List<int> nonMatchedUsers = users.Where(id => !db.Users.Any(u => u.UserID == id)).ToList();
                    return Json("Error - users ("+nonMatchedUsers.ToString()+") don't exist in database!", JsonRequestBehavior.AllowGet);
                }
                problem.AssignedUsers = assignedUsers;
                db.Problems.Add(problem);
                try
                {
                    db.SaveChanges();
                } catch(Exception e)
                {
                    string mess = e.Message;
                }
            }
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStories()
        {
            using (db)
            {
                var stories = db.UserStories
                    .Select(t => new
                    {
                        id = t.UserStoryID,
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

        [HttpGet]
        public ActionResult AllPeople()
        {
            using (db)
            {
                var people = db.Users
                        .Select(p => new
                        {
                            id = p.UserID,
                            NAME = p.Name,
                        });
                return Json(people.ToList(), JsonRequestBehavior.AllowGet);
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