using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaseSlovoApp.Models;

namespace NaseSlovoApp.Controllers
{
    public class KnjigeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Knjige
        public ActionResult Index()
        {
            var knjige = db.Knjige.Include(k => k.Vrsta);
            return View(knjige.ToList());
        }

        // GET: Knjige/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjige.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            return View(knjiga);
        }

        public ActionResult Top()
        {
            var knjige = db.Knjige.Include(k => k.Vrsta);
            // rangiraj po srednjoj ocjeni i odaberi prvih 10
            knjige = knjige
                .OrderByDescending(k => k.SrednjaOcjena)
                .Take(10);
            
            return View("Index", knjige.ToList());
        }

        // GET: Knjige/Create
        public ActionResult Create()
        {
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv");
            return View();
        }

        // POST: Knjige/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KnjigaID,Autori,Naslov,Signatura,Izdavac,Godina,VrstaID")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                knjiga.SrednjaOcjena = 0;
                db.Knjige.Add(knjiga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", knjiga.VrstaID);
            return View(knjiga);
        }

        // GET: Knjige/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjige.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", knjiga.VrstaID);
            return View(knjiga);
        }

        // POST: Knjige/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KnjigaID,Autori,Naslov,Signatura,Izdavac,Godina,VrstaID,SrednjaOcjena")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knjiga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", knjiga.VrstaID);
            return View(knjiga);
        }

        // GET: Knjige/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjige.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            return View(knjiga);
        }

        // POST: Knjige/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Knjiga knjiga = db.Knjige.Find(id);
            db.Knjige.Remove(knjiga);
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
