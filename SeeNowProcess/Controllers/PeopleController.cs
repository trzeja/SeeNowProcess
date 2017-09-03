using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using System.Data.Entity.Infrastructure;
using SeeNowProcess.Models.Enums;

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

        public ActionResult PeopleIndex(int? count)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            using (db)
            {
                var allUsers = db.Users;
                if (count != null)
                {
                    allUsers.Take((int)count);                    
                }

                return View(allUsers.ToList());
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

        [HttpPost]
        public ActionResult GetUserInfo(int id)
        {
            using (db)
            {
                var people = db.Users.Where (u => u.UserID == id)
                        .Select(p => new
                        {
                            id = p.UserID,
                            login = p.Login,
                            name = p.Name,
                            email = p.Email,
                            phone = p.PhoneNumber,
                            role = p.role
                        });
                return Json(people.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [Route("{id}")]
        public ActionResult Show(int id)
        {
            using (db)
            {
                var allUsers = db.Users.Where(u => u.UserID == id);                            
                return View(allUsers.ToList());
                //db.MarkAsModified<User>(allUsers.FirstOrDefault()); //Sample of mark as modified 
            }
        }
        [HttpPost]
        public ActionResult GetTasks(int userId)
        {
            using (db)
            {
                var resultJ = db.Problems
                    .Where(p => p.AssignedUsers.Select(u => u.UserID).Contains(userId))
                    .Select(p => new
                    {
                        ProblemID = p.ProblemID,
                        Title = p.Title,
                        Description = p.Description,
                        Importance = p.Importance,
                        Creation = p.CreationDate,
                        Story = p.Story.Title,
                        BoxOrder = p.Box.Order,
                        AssignedUsers = p.AssignedUsers.Select(u => u.UserID).ToList()
                    });
                return new JsonResult
                {
                    Data = new
                    {
                        UserID = userId,
                        Tasks = resultJ.ToList()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult AssignTask(int userId, int taskId)
        {
            using (db)
            {
                Problem problem = db.Problems.Where(p => p.ProblemID == taskId).FirstOrDefault();
                User user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();
                if (problem == null)
                    return Json("Invalid Task", JsonRequestBehavior.AllowGet);
                if (user == null)
                    return Json("Invalid User", JsonRequestBehavior.AllowGet);
                user.Problems.Add(problem);
                db.MarkAsModified(problem);
                db.MarkAsModified(user);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RevokeTask(int userId, int taskId)
        {
            using (db)
            {
                Problem problem = db.Problems.Where(p => p.ProblemID == taskId).FirstOrDefault();
                User user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();
                if (problem == null)
                    return Json("Invalid Task", JsonRequestBehavior.AllowGet);
                if (user == null)
                    return Json("Invalid User", JsonRequestBehavior.AllowGet);
                if (!user.Problems.Contains(problem))
                    return Json("Given user isn't assigned to given task", JsonRequestBehavior.AllowGet);
                user.Problems.Remove(problem);
                db.MarkAsModified(problem);
                db.MarkAsModified(user);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AssignTeam(int userId, int teamId)
        {
            using (db)
            {
                Team team = db.Teams.Where(p => p.TeamID == teamId).FirstOrDefault();
                User user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();
                if (team == null)
                    return Json("Invalid Team", JsonRequestBehavior.AllowGet);
                if (user == null)
                    return Json("Invalid User", JsonRequestBehavior.AllowGet);
                user.Assignments.Add(new Assignment{Team=team, User=user});
                db.MarkAsModified(team);
                db.MarkAsModified(user);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RevokeTeam(int userId, int teamId)
        {
            using (db)
            {
                Team team = db.Teams.Where(p => p.TeamID == teamId).FirstOrDefault();
                User user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();
                if (team == null)
                    return Json("Invalid Team", JsonRequestBehavior.AllowGet);
                if (user == null)
                    return Json("Invalid User", JsonRequestBehavior.AllowGet);
                Assignment assignment = user.Assignments.Where(a => a.TeamID == team.TeamID).FirstOrDefault();
                if (assignment == null)
                    return Json("Given user isn't assigned to given team", JsonRequestBehavior.AllowGet);
                db.Assignments.Remove(assignment);
                db.MarkAsModified(team);
                db.MarkAsModified(user);
                    db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetAllTasks()
        {
            using (db)
            {
                var Users = db.Users.Select(u => u.UserID).ToList();
                var data = new List<JsonResult>();
                Users.ForEach(u =>
                {
                    data.Add((JsonResult)GetTasks(u));
                });
                JsonResult result = new JsonResult
                {
                    Data = data.ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return result;
            }
        }
        public ActionResult GetAssignments(int userId)
        {
            using (db)
            {
                var Teams = db.Assignments
                    .Where(a => a.UserID == userId)
                    .Select(a => a.Team)
                    .Select(t => new
                    {
                        TeamID = t.TeamID,
                        Name = t.Name,
                        UserStoryID = t.UserStory == null ? -1 : t.UserStory.UserStoryID,
                        UserStoryTitle = t.UserStory == null ? "Unassigned" : t.UserStory.Title,
                        UserStoryDescription = t.UserStory == null ? "Unassigned" : t.UserStory.Description,
                        /*UserStorySize = t.UserStory == null ? "Unassigned" : t.UserStory.Size,*/
                        UserStoryUnit = t.UserStory == null ? "Unassigned" : t.UserStory.Unit,
                        UserStoryProject = t.UserStory == null ? "Unassigned" : t.UserStory.Project.Name,
                        Leader = t.TeamLeader.Name,
                        ProjectID = t.UserStory == null ? -1 : t.UserStory.Project.ProjectID,
                        ProjectName = t.UserStory == null ? "Unassigned" : t.UserStory.Project.Name,
                        ProjectDescription = t.UserStory == null ? "Unassigned" : t.UserStory.Project.Description,
                        ProjectStartDate = t.UserStory == null ? "Unassigned" : t.UserStory.Project.StartDate.ToString(),
                        ProjectCompletionDate = t.UserStory == null ? "Unassigned" : t.UserStory.Project.CompletionDate.ToString(),
                        ProjectStatus = t.UserStory == null ? "Unassigned" : t.UserStory.Project.Status.ToString()
                    });
                //przypisań do projektu chyba nie robimy?
                return new JsonResult
                {
                    Data = new
                    {
                        UserID = userId,
                        Teams = Teams.ToList()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult deleteUser(int id) {
            using (db)
            {
                User user = db.Users.Where(u => u.UserID == id).FirstOrDefault();
                if (user == null)
                    return Json("No such user!", JsonRequestBehavior.AllowGet);
                foreach(Problem problem in user.Problems)
                {
//                    problem.AssignedUsers.Remove(user);
                    db.MarkAsModified(problem);
                }
                foreach(User subordinate in user.Subordinates)
                {
//                    subordinate.Supervisor = user.Supervisor;
                    db.MarkAsModified(subordinate);
                }
                IEnumerable<Assignment> assignments = db.Assignments.Where(a => a.UserID == user.UserID).ToList();
                db.Assignments.RemoveRange(assignments);
                IEnumerable<Team> teams = db.Teams.Where(t => t.TeamLeader.UserID == user.UserID).ToList();
                foreach(Team team in teams)
                {
                    team.TeamLeader = null;
                    db.MarkAsModified(team);
                }

                user.Problems.Clear();
                //userstories?
                //user.Stories.Clear();
                user.Subordinates.Clear();
                //user.Assignments.Clear();
                db.Users.Remove(user);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var x = e.InnerException;
                }
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult updateUserData(int id, string name, string login, string email, string phone, Role role)
        {
            using (db)
            {
                User user = db.Users.Where(u => u.UserID == id).FirstOrDefault();
                if (user == null)
                    return Json("No such user!", JsonRequestBehavior.AllowGet);
                if (user.Login != login)
                    if (db.Users.Any(u => u.Login.Equals(login)))
                        return Json("Given login is busy!", JsonRequestBehavior.AllowGet);
                user.Login = login;
                user.Name = name;
                user.Email = email;
                user.PhoneNumber = phone;
                user.role = role;
                db.MarkAsModified(user);
                db.SaveChanges();
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult updateUserPassword(int id, string oldPassword, string newPassword)
        {
            using (db)
            {
                User user = db.Users.Where(u => u.UserID == id).FirstOrDefault();
                if (user == null)
                    return Json("No such user!", JsonRequestBehavior.AllowGet);
                if (!user.ComparePassword(oldPassword))
                    return Json("Invalid password!", JsonRequestBehavior.AllowGet);
                user.Password = newPassword;
                db.MarkAsModified(user);
                db.SaveChanges();
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RevokeStory(int userId, int storyId)
        {
            /*     using (db)
                 {
                     Problem problem = db.Problems.Where(p => p.ProblemID == taskId).FirstOrDefault();
                     User user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();
                     if (problem == null)
                         return Json("Invalid Task", JsonRequestBehavior.AllowGet);
                     if (user == null)
                         return Json("Invalid User", JsonRequestBehavior.AllowGet);
                     if (!user.Problems.Contains(problem))
                         return Json("Given user isn't assigned to given task", JsonRequestBehavior.AllowGet);
                     user.Problems.Remove(problem);
                     db.MarkAsModified(problem);
                     db.MarkAsModified(user);
                     db.SaveChanges();
                     return Json("Success", JsonRequestBehavior.AllowGet);
                 }*/
            return Json("Success", JsonRequestBehavior.AllowGet);
        }


    }
}