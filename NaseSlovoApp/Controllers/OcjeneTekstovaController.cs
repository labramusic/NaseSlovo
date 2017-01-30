using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaseSlovoApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NaseSlovoApp.Controllers
{
    public class OcjeneTekstovaController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        protected ApplicationDbContext ApplicationDbContext { get; set; }
        
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public OcjeneTekstovaController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: OcjeneTekstova
        public ActionResult Index()
        {
            var ocjeneTekstova = db.OcjeneTekstova.Include(o => o.Korisnik).Include(o => o.Tekst);
            return View(ocjeneTekstova.ToList());
        }

        // GET: OcjeneTekstova/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaTeksta ocjenaTeksta = db.OcjeneTekstova.Find(id);
            if (ocjenaTeksta == null)
            {
                return HttpNotFound();
            }
            return View(ocjenaTeksta);
        }

        // GET: OcjeneTekstova/Create
        public ActionResult Create()
        {
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime");
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj");
            return View();
        }

        // POST: OcjeneTekstova/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ocjena")] OcjenaTeksta ocjenaTeksta)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);

                ocjenaTeksta.KorisnikID = user.KorisnikID;
                ocjenaTeksta.TekstID = Int32.Parse(Request.Form.Get("TekstID"));

                if (db.OcjeneTekstova
                    .Any(o => o.KorisnikID == ocjenaTeksta.KorisnikID
                    && o.TekstID == ocjenaTeksta.TekstID))
                {
                    db.OcjeneTekstova.Where(o => o.KorisnikID == ocjenaTeksta.KorisnikID
                    && o.TekstID == ocjenaTeksta.TekstID).First().Ocjena = ocjenaTeksta.Ocjena;
                }
                else
                {
                    db.OcjeneTekstova.Add(ocjenaTeksta);
                }
                
                db.SaveChanges();

                // racunaj srednju ocjenu
                // spremi promjene u bazu
                Tekst tekst = db.Tekstovi.Find(ocjenaTeksta.TekstID);
                double srednjaOcjena = tekst.SrednjaOcjena;
                srednjaOcjena = (double)tekst.OcjeneTekstova.Sum(o => o.Ocjena) / tekst.OcjeneTekstova.Count();
                tekst.SrednjaOcjena = srednjaOcjena;
                db.SaveChanges();

                // TODO isto kao komentaricontroller
                return RedirectToAction("Details", "Tekstovi", new { id = tekst.TekstID });
            }

            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaTeksta.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", ocjenaTeksta.TekstID);
            return View(ocjenaTeksta);
        }

        // GET: OcjeneTekstova/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaTeksta ocjenaTeksta = db.OcjeneTekstova.Find(id);
            if (ocjenaTeksta == null)
            {
                return HttpNotFound();
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaTeksta.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", ocjenaTeksta.TekstID);
            return View(ocjenaTeksta);
        }

        // POST: OcjeneTekstova/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TekstID,KorisnikID,Ocjena")] OcjenaTeksta ocjenaTeksta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocjenaTeksta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaTeksta.KorisnikID);
            ViewBag.TekstID = new SelectList(db.Tekstovi, "TekstID", "Sadrzaj", ocjenaTeksta.TekstID);
            return View(ocjenaTeksta);
        }

        // GET: OcjeneTekstova/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaTeksta ocjenaTeksta = db.OcjeneTekstova.Find(id);
            if (ocjenaTeksta == null)
            {
                return HttpNotFound();
            }
            return View(ocjenaTeksta);
        }

        // POST: OcjeneTekstova/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OcjenaTeksta ocjenaTeksta = db.OcjeneTekstova.Find(id);
            db.OcjeneTekstova.Remove(ocjenaTeksta);
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
