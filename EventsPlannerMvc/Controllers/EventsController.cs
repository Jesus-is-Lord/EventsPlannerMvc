using EventsPlannerMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace EventsPlannerMvc.Controllers
{
    public class EventsController : Controller
    {
        private EventsPlannerContext db = new EventsPlannerContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return a viewmodel with list of current events for domain user
                //list of events for which current domain user is owner
                var currentLoggedInUsername = User.Identity.GetUserName();
                var currentLoggedInUser = db.Users.Where(u => u.Username.Equals(currentLoggedInUsername)).First();
                var e = db.Events.Where(ev => ev.EventOwner.Equals(currentLoggedInUser.Id));
                //list of events for which members table has user as current domain user
                var membersWithDomainUser = db.Members.Where(m => m.MemberOfUser.Equals(currentLoggedInUser.Id));
                var query = from ev in db.Events
                            join mem in membersWithDomainUser on ev.Id equals mem.MemberOfEvent
                            select ev;
                //
                var result = e.Union(query);//.OrderBy(c => c.EventDate.Date).ThenBy(c => c.EventDate.TimeOfDay);
                return View(result.ToList());
            }
            else
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Events1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EventDate,EventOwner")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.Id = Guid.NewGuid();
                //set the event owner to current user
                var currentLoggedInUsername = User.Identity.GetUserName();
                var currentLoggedInUser = db.Users.Where(u => u.Username.Equals(currentLoggedInUsername)).First();
                @event.EventOwner = currentLoggedInUser.Id;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
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