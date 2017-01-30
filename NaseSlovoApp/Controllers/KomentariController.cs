using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaseSlovoApp.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NaseSlovoApp.Controllers
{
    public class KomentariController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public KomentariController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: Komentari
        public ActionResult Index()
        {
            var komentari = db.Komentari.Include(k => k.Korisnik).Include(k => k.Tekst);
            return View(komentari.ToList());
        }

        // GET: Komentari/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentar komentar = db.Komentari.Find(id);
            if (komentar == null)
            {
                return HttpNotFound();
            }
            return View(komentar);
        }

        // GET: Komentari/Create
        public ActionResult Create()
        {
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime");
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj");
            return View();
        }

        // POST: Komentari/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sadrzaj")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);
                //var user = UserManager.FindByIdAsync(User.Identity.GetUserId());
                //ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.User.Identity.GetUserId());
                
                komentar.KorisnikID = user.KorisnikID;
                komentar.TekstID = Int32.Parse(Request.Form.Get("TekstID"));
                komentar.DatumVrijeme = DateTime.Now;
                db.Komentari.Add(komentar);
                db.SaveChanges();
                // TODO
                return RedirectToAction("Details", "Tekstovi", new { id = komentar.TekstID });
            }

            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", komentar.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", komentar.TekstID);
            return View(komentar);
        }

        public ActionResult LektorKom([Bind(Include = "Sadrzaj")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);
                Tekst tekst = db.Tekstovi.Find(Int32.Parse(Request.Form["TekstID"]));
                tekst.LektorovKomentar = komentar.Sadrzaj;
                db.SaveChanges();
                return RedirectToAction("Details", "Tekstovi", new { id = tekst.TekstID });
            }
            return HttpNotFound();
        }

        // GET: Komentari/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentar komentar = db.Komentari.Find(id);
            if (komentar == null)
            {
                return HttpNotFound();
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", komentar.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", komentar.TekstID);
            return View(komentar);
        }

        // POST: Komentari/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KomentarID,KorisnikID,TekstID,Sadrzaj,DatumVrijeme")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(komentar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", komentar.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", komentar.TekstID);
            return View(komentar);
        }

        // GET: Komentari/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentar komentar = db.Komentari.Find(id);
            if (komentar == null)
            {
                return HttpNotFound();
            }
            return View(komentar);
        }

        // POST: Komentari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Komentar komentar = db.Komentari.Find(id);
            db.Komentari.Remove(komentar);
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
