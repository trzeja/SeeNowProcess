using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;
using SeeNowProcess.DAL;
using System.Data.Entity.Infrastructure;

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
                var allUsers = db.Users;
                if (count != null)
                {
                    allUsers.Take((int)count);                    
                }

                return View(allUsers.ToList());
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
                        Leader = t.TeamLeader.Name
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
    }
}