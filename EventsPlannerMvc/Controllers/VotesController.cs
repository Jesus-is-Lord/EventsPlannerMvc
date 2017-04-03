using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventsPlannerMvc.Models;

namespace EventsPlannerMvc.Controllers
{
    public class VotesController : Controller
    {
        private EventsPlannerContext db = new EventsPlannerContext();

        // GET: Votes
        public ActionResult Index(Guid id)
        {
            var votes = db.Votes.Where(v => v.Idea == id);
            return View(votes.ToList());
        }

        // GET: Votes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        // GET: Votes/Create
        public ActionResult Create(Guid memberID, Guid ideaID)
        {
            ViewBag.VoteCode = new SelectList(db.VoteCodes.OrderBy(x => x.Value), "Id", "Code");

            //if this member voted for the idea before, update the vote with latest
            var vv = db.Votes.Where(v => v.Voter == memberID && v.Idea == ideaID).FirstOrDefault();
            if (vv != null)
                return View(vv);
            else
            {
                Vote v = new Vote();
                var idea = db.Ideas.Where(i => i.Id == ideaID).First();
                var member = db.Members.Where(m => m.Id == memberID).First();
                v.Idea = ideaID;
                v.Voter = memberID;
                v.Idea1 = idea;
                v.Member = member;


                return View(v);
            }
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Idea,VoteCode,Voter")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                //check to see if it is a new vote or an overwriting one
                if (db.Votes.Any(v => v.Id == vote.Id))
                {
                    db.Entry(vote).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    vote.Id = Guid.NewGuid();
                    db.Votes.Add(vote);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { id = vote.Idea });
            }
            ViewBag.VoteCode = new SelectList(db.VoteCodes.OrderBy(x => x.Value), "Id", "Code");
            return View(vote);
        }

        // GET: Votes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idea = new SelectList(db.Ideas, "Id", "Title", vote.Idea);
            ViewBag.Voter = new SelectList(db.Members, "Id", "Id", vote.Voter);
            ViewBag.VoteCode = new SelectList(db.VoteCodes, "Id", "Code", vote.VoteCode);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Idea,VoteCode,Voter")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Idea = new SelectList(db.Ideas, "Id", "Title", vote.Idea);
            ViewBag.Voter = new SelectList(db.Members, "Id", "Id", vote.Voter);
            ViewBag.VoteCode = new SelectList(db.VoteCodes, "Id", "Code", vote.VoteCode);
            return View(vote);
        }

        // GET: Votes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vote vote = db.Votes.Find(id);
            if (vote == null)
            {
                return HttpNotFound();
            }
            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Vote vote = db.Votes.Find(id);
            db.Votes.Remove(vote);
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
