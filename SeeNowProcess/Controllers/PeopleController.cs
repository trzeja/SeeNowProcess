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
    }
}