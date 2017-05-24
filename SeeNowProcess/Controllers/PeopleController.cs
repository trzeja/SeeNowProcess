﻿using System;
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
                user.Assignments.Remove(assignment);
                db.MarkAsModified(team);
                db.MarkAsModified(user);
                db.MarkAsModified(assignment); // to tu ma być czy nie? - wyjdzie na testach jak będzie front
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
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