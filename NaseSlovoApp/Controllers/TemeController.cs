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
    public class TemeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Teme
        public ActionResult Index()
        {
            return View(db.Teme.ToList());
        }

        // GET: Teme/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Teme.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // GET: Teme/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teme/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TemaID,Naziv")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                db.Teme.Add(tema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tema);
        }

        // GET: Teme/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Teme.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Teme/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TemaID,Naziv")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tema);
        }

        // GET: Teme/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Teme.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Teme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tema tema = db.Teme.Find(id);
            db.Teme.Remove(tema);
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
