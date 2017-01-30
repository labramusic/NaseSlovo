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
    public class OcjeneKnjigaController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public OcjeneKnjigaController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: OcjeneKnjiga
        public ActionResult Index()
        {
            var ocjeneKnjiga = db.OcjeneKnjiga.Include(o => o.Knjiga).Include(o => o.Korisnik);
            return View(ocjeneKnjiga.ToList());
        }

        // GET: OcjeneKnjiga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaKnjige ocjenaKnjige = db.OcjeneKnjiga.Find(id);
            if (ocjenaKnjige == null)
            {
                return HttpNotFound();
            }
            return View(ocjenaKnjige);
        }

        // GET: OcjeneKnjiga/Create
        public ActionResult Create()
        {
            ViewBag.KnjigaID = new SelectList(db.Knjige, "KnjigaID", "Autori");
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime");
            return View();
        }

        // POST: OcjeneKnjiga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ocjena")] OcjenaKnjige ocjenaKnjige)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);

                ocjenaKnjige.KorisnikID = user.KorisnikID;
                ocjenaKnjige.KnjigaID = Int32.Parse(Request.Form.Get("KnjigaID"));

                if (db.OcjeneKnjiga
                    .Any(o => o.KorisnikID == ocjenaKnjige.KorisnikID
                    && o.KnjigaID == ocjenaKnjige.KnjigaID))
                {
                    db.OcjeneKnjiga.Where(o => o.KorisnikID == ocjenaKnjige.KorisnikID
                    && o.KnjigaID == ocjenaKnjige.KnjigaID).First().Ocjena = ocjenaKnjige.Ocjena;
                }
                else
                {
                    db.OcjeneKnjiga.Add(ocjenaKnjige);
                }

                db.SaveChanges();

                // racunaj srednju ocjenu
                Knjiga knjiga = db.Knjige.Find(ocjenaKnjige.KnjigaID);
                double srednjaOcjena = knjiga.SrednjaOcjena;
                srednjaOcjena = (double)knjiga.OcjeneKnjiga.Sum(o => o.Ocjena) / knjiga.OcjeneKnjiga.Count();
                knjiga.SrednjaOcjena = srednjaOcjena;
                db.SaveChanges();

                // TODO isto kao ocjenetekstovacontroller
                return RedirectToRoute("/Knjige/Index/");
            }

            ViewBag.KnjigaID = new SelectList(db.Knjige, "KnjigaID", "Autori", ocjenaKnjige.KnjigaID);
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaKnjige.KorisnikID);
            return View(ocjenaKnjige);
        }

        // GET: OcjeneKnjiga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaKnjige ocjenaKnjige = db.OcjeneKnjiga.Find(id);
            if (ocjenaKnjige == null)
            {
                return HttpNotFound();
            }
            ViewBag.KnjigaID = new SelectList(db.Knjige, "KnjigaID", "Autori", ocjenaKnjige.KnjigaID);
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaKnjige.KorisnikID);
            return View(ocjenaKnjige);
        }

        // POST: OcjeneKnjiga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KnjigaID,KorisnikID,Ocjena")] OcjenaKnjige ocjenaKnjige)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ocjenaKnjige).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KnjigaID = new SelectList(db.Knjige, "KnjigaID", "Autori", ocjenaKnjige.KnjigaID);
            ViewBag.KorisnikID = new SelectList(db.Korisnici, "KorisnikID", "Ime", ocjenaKnjige.KorisnikID);
            return View(ocjenaKnjige);
        }

        // GET: OcjeneKnjiga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OcjenaKnjige ocjenaKnjige = db.OcjeneKnjiga.Find(id);
            if (ocjenaKnjige == null)
            {
                return HttpNotFound();
            }
            return View(ocjenaKnjige);
        }

        // POST: OcjeneKnjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OcjenaKnjige ocjenaKnjige = db.OcjeneKnjiga.Find(id);
            db.OcjeneKnjiga.Remove(ocjenaKnjige);
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
