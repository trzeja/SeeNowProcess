using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeeNowProcess.Models;

namespace SeeNowProcess.Controllers
{
    public class SimpleTasksController : Controller
    {
        private SimpleTaskDBContext db = new SimpleTaskDBContext();

        // GET: SimpleTasks
        //public ActionResult Index()
        //{
        //    return View(db.SimpleTasks.ToList());
        //}

        // GET: SimpleTasks
        public ActionResult Index(string importance, string searchString)
        {
            var ImportanceLst = new List<string>();

            var ImportanceQry = from d in db.SimpleTasks
                           orderby d.Importance
                           select d.Importance.ToString();           
            
            ImportanceLst.AddRange(ImportanceQry.Distinct());
            ViewBag.importance = new SelectList(ImportanceLst);

            var simpleTasks = from s in db.SimpleTasks
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                simpleTasks = simpleTasks.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(importance))
            {
                simpleTasks = simpleTasks.Where(x => x.Importance.ToString() == importance);
            }

            return View(simpleTasks);
        }



        // GET: SimpleTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Problem simpleTask = db.SimpleTasks.Find(id);
            if (simpleTask == null)
            {
                return HttpNotFound();
            }
            return View(simpleTask);
        }

        // GET: SimpleTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SimpleTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Importance,Title,Description,CurrentState,CreationDate,CompletionDate,Progress,Comments")] Problem simpleTask)
        {
            if (ModelState.IsValid)
            {
                db.SimpleTasks.Add(simpleTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(simpleTask);
        }

        // GET: SimpleTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Problem simpleTask = db.SimpleTasks.Find(id);
            if (simpleTask == null)
            {
                return HttpNotFound();
            }
            return View(simpleTask);
        }

        // POST: SimpleTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Importance,Title,Description,CurrentState,CreationDate,CompletionDate,Progress,Comments")] Problem simpleTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(simpleTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(simpleTask);
        }

        // GET: SimpleTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Problem simpleTask = db.SimpleTasks.Find(id);
            if (simpleTask == null)
            {
                return HttpNotFound();
            }
            return View(simpleTask);
        }

        // POST: SimpleTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Problem simpleTask = db.SimpleTasks.Find(id);
            db.SimpleTasks.Remove(simpleTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
