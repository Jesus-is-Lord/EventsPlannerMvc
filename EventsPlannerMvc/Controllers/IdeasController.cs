using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventsPlannerMvc.Models;
using Microsoft.AspNet.Identity;

namespace EventsPlannerMvc.Controllers
{
    public class IdeasController : Controller
    {
        private EventsPlannerContext db = new EventsPlannerContext();

        // GET: Ideas
        public ActionResult Index()
        {
            List<Idea> ideas = new List<Idea>();
            var loggedInUsername = User.Identity.GetUserName();
            var loggedInUserID = db.Users.Where(u => u.Username.Equals(loggedInUsername)).First().Id;
            var memberEntries = db.Members.Where(m => m.MemberOfUser == loggedInUserID);
            foreach(var m in memberEntries)
            {
                //get all ideas belonging to the current event
                var e = m.Event.Members;
                foreach(var mm in e)
                {
                    ideas.AddRange(mm.Ideas);
                }
            }

            return View(ideas);
        }

        // GET: Ideas/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // GET: Ideas/Create
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var loggedInUsername = User.Identity.GetUserName();
            var member = db.Members.Where(m => m.User.Username.Equals(loggedInUsername) && m.MemberOfEvent == id).First();
            Idea idea = new Idea();
            idea.IdeaOwner = member.Id;
            return View(idea);
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,IdeaOwner")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                idea.Id = Guid.NewGuid();
                db.Ideas.Add(idea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(idea);
        }

        // GET: Ideas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdeaOwner = new SelectList(db.Members, "Id", "Id", idea.IdeaOwner);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,IdeaOwner")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdeaOwner = new SelectList(db.Members, "Id", "Id", idea.IdeaOwner);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
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
