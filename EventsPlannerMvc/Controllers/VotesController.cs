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
    public class VotesController : Controller
    {
        private EventsPlannerContext db = new EventsPlannerContext();

        // GET: Votes
        public ActionResult Index(Guid id)
        {
            var votes = db.Votes.Where(v => v.Idea == id);
            return View(votes.ToList());
        }

        // GET: Votes/Create
        public ActionResult Create(Guid memberID, Guid ideaID)
        {
            ViewBag.VoteCode = new SelectList(db.VoteCodes.OrderBy(x => x.Value), "Id", "Code");

            //if this member voted for the idea before, update the vote with latest
            var loggedInUsername = User.Identity.GetUserName();
            var loggedInUserId = db.Users.Where(u => u.Username.Equals(loggedInUsername)).First().Id;
            var idea = db.Ideas.Where(i => i.Id == ideaID).First();
            var member = db.Members.Where(m => m.Id == memberID).First();
            var rightMember = db.Members.Where(m => m.MemberOfEvent == member.MemberOfEvent && m.MemberOfUser == loggedInUserId).First();

            var vv = db.Votes.Where(v => v.Voter == rightMember.Id && v.Idea == ideaID).FirstOrDefault();
            if (vv != null)
                return View(vv);
            else
            {
                Vote v = new Vote();

                v.Idea = ideaID;
                v.Voter = rightMember.Id;
                v.Idea1 = idea;
                v.Member = rightMember;


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
