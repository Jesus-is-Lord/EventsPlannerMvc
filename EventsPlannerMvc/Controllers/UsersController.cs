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
using System.Text;
using System.IO;

namespace EventsPlannerMvc.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private EventsPlannerContext db = new EventsPlannerContext();

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public JsonResult IsUserFound(string id)
        {
            byte[] data = Convert.FromBase64String(id);
            string decodedString = Encoding.UTF8.GetString(data);

            var model = db.Users.Where(u => u.Username.Equals(decodedString)).FirstOrDefault();
            List<Boolean> result = new List<Boolean> { };
            if (model != null)
                result.Add(true);

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult IsUserProfilePhotoFound()
        {
            List<string> value = new List<string> { };
            var result = Json(new { Data = value }, JsonRequestBehavior.AllowGet);

            var loggedInUsername = User.Identity.GetUserName();

            var model = db.Users.Where(u => u.Username.Equals(loggedInUsername)).FirstOrDefault();
            if (model != null && model.ProfilePhoto!=null)
                value.Add(Convert.ToBase64String(model.ProfilePhoto));

            return result;
        }

        

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            User user = null;
            if (id == null)
            {
                var loggedInUsername = User.Identity.GetUserName();
                user = db.Users.Where(u => u.Username.Equals(loggedInUsername)).First();
            }
            else
                user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Firstname,Lastname,ProfilePhoto")] User user, HttpPostedFileBase image)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                HttpPostedFileWrapper im = (HttpPostedFileWrapper)Request.Files[0];
                if (!String.IsNullOrEmpty(im.FileName) && im.ContentType.Equals("image/jpeg"))
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(im.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }
                    user.ProfilePhoto = fileData;
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Events");
            }
            return View(user);
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
