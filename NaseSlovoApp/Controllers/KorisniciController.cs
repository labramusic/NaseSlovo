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
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NaseSlovoApp.Controllers
{
	public class KorisniciController : Controller
	{
		private DatabaseEntities db = new DatabaseEntities();

		protected ApplicationDbContext ApplicationDbContext { get; set; }

		protected UserManager<ApplicationUser> UserManager { get; set; }

		public KorisniciController()
		{
			this.ApplicationDbContext = new ApplicationDbContext();
			this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
		}

		// GET: Korisnici
		public ActionResult Index()
		{
			return View(db.Korisnici.ToList());
		}
		
		public ActionResult Clanovi()
		{
			var korisnici = db.Korisnici.Where(u => u.Clanstva.Count != 0);
			
			return View(korisnici.ToList());
		}

		public ActionResult DodijeliAutora(int id)
		{
			Korisnik korisnik = db.Korisnici.Find(id);
			korisnik.Autor = !korisnik.Autor;
			db.SaveChanges();
			return RedirectToAction("Clanovi");
		}

		// GET: Korisnici/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Korisnik korisnik = db.Korisnici.Find(id);
			if (korisnik == null)
			{
				return HttpNotFound();
			}
			return View(korisnik);
		}

		// GET: Korisnici/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Korisnici/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "KorisnikID,Ime,Prezime,DatumRod,Email,Spol")] Korisnik korisnik)
		{
			if (ModelState.IsValid)
			{
				db.Korisnici.Add(korisnik);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(korisnik);
		}

		// GET: Korisnici/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Korisnik korisnik = db.Korisnici.Find(id);
			if (korisnik == null)
			{
				return HttpNotFound();
			}
			return View(korisnik);
		}

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		// POST: Korisnici/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "KorisnikID,Ime,Prezime,DatumRod,Email,Spol")] Korisnik korisnik)
		{
			if (ModelState.IsValid)
			{
				db.Entry(korisnik).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(korisnik);
		}

		// GET: Korisnici/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Korisnik korisnik = db.Korisnici.Find(id);
			if (korisnik == null)
			{
				return HttpNotFound();
			}
			return View(korisnik);
		}

		// POST: Korisnici/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Korisnik korisnik = db.Korisnici.Find(id);
			db.Korisnici.Remove(korisnik);
			db.SaveChanges();
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			ApplicationUser user = ApplicationDbContext.Users.Where(u => u.KorisnikID == id).First();
			UserManager.Delete(user);
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
