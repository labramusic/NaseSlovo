using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NaseSlovoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace NaseSlovoApp.Controllers
{
    public class SubscriptionController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        private ApplicationSignInManager _signInManager;

        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public SubscriptionController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: Subscription
        public ActionResult Index()
        {
            return View();
        }


        // GET: Subscription/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscription/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Subscription/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Subscription/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Subscription/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

        // GET
        public ActionResult AuthorSub()
        {
            // TODO posalji model authori aslist
            var autori = db.Korisnici.Where(t => t.Autor==true);
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            var tmp = korisnik.Pretplaceni.ToList();
            List<int> pretplaceni = new List<int>();
            foreach(Korisnik k in tmp)
            {
                pretplaceni.Add(k.KorisnikID);   
            }
            ViewBag.pret = pretplaceni;
            return View(autori.ToList());
        }

        // POST
        [HttpPost]
        public ActionResult AuthorSub(Korisnik autor)
        {
            var autorID = Request.Form.Get("KorisnikID");
            autor = db.Korisnici.Find(Int32.Parse(autorID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Autori.Add(autor);
                db.SaveChanges();
            }
            return RedirectToAction("AuthorSub");
        }
        [HttpPost]
        public ActionResult CancelAuthor()
        {
            var autorID = Request.Form.Get("KorisnikID");
            Korisnik autor = db.Korisnici.Find(Int32.Parse(autorID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Autori.Remove(autor);
                db.SaveChanges();
            }
            return RedirectToAction("AuthorSub");
        }

        public ActionResult TypeSub()
        {
            var vrste = db.Vrste;
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            var tmp = korisnik.Vrste.ToList();
            List<int> pretplaceni = new List<int>();
            foreach (Vrsta k in tmp)
            {
                pretplaceni.Add(k.VrstaID);
            }
            ViewBag.pret = pretplaceni;
            return View(vrste.ToList());
        }

        [HttpPost]
        public ActionResult TypeSub(Vrsta v)
        {
            var vrstaID = Request.Form.Get("VrstaID");
            Vrsta vrsta = db.Vrste.Find(Int32.Parse(vrstaID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Vrste.Add(vrsta);
                db.SaveChanges();
            }
            return RedirectToAction("TypeSub");
        }

        [HttpPost]
        public ActionResult CancelType()
        {
            var vrstaID = Request.Form.Get("VrstaID");
            Vrsta vrsta = db.Vrste.Find(Int32.Parse(vrstaID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Vrste.Remove(vrsta);
                db.SaveChanges();
            }
            return RedirectToAction("TypeSub");
        }


        public ActionResult ThemeSub()
        {
            var teme = db.Teme;
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            var tmp = korisnik.Teme.ToList();
            List<int> pretplaceni = new List<int>();
            foreach (Tema k in tmp)
            {
                pretplaceni.Add(k.TemaID);
            }
            ViewBag.pret = pretplaceni;
            return View(teme.ToList());
        }

        [HttpPost]
        public ActionResult ThemeSub(Tema t)
        {
            var temaID = Request.Form.Get("TemaID");
            Tema tema = db.Teme.Find(Int32.Parse(temaID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Teme.Add(tema);
                db.SaveChanges();
            }
            return RedirectToAction("ThemeSub");
        }

        [HttpPost]
        public ActionResult CancelTheme()
        {
            var temaID = Request.Form.Get("TemaID");
            Tema tema = db.Teme.Find(Int32.Parse(temaID));
            // dodaj autora u korisnikove pretplate (trenutnog)
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            if (ModelState.IsValid)
            {
                korisnik.Teme.Remove(tema);
                db.SaveChanges();
            }
            return RedirectToAction("ThemeSub");
        }

        // GET : Subscription/Membership
        public ActionResult Membership()
        {
            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // POST: Subscription/Membership
        [HttpPost]
        public ActionResult Membership(FormCollection collection)
        {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var id = User.Identity.GetUserId();
                var roles=UserManager.GetRoles(id);
                UserManager.RemoveFromRoles(id,"Simpatizer");
                if (!User.IsInRole("Clan"))
                {
                    UserManager.AddToRole(id, "Clan");
                }
                String pret=Request.Form.Get("Pretplata");
                Clanstvo clanstvo = new Clanstvo();
                id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);
                Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
                clanstvo.KorisnikID = korisnik.KorisnikID;
                clanstvo.DatumPlat = DateTime.Now;
                if (pret == "month")
                {
                if (korisnik.Clanstva.Count()==0)
                {
                    clanstvo.DatumIstek = DateTime.Now.AddMonths(1);
                } else
                {
                    clanstvo.DatumIstek=korisnik.Clanstva.First().DatumIstek.AddMonths(1);
                }
                } else
                {
                if (korisnik.Clanstva.Count()==0)
                {
                    clanstvo.DatumIstek = DateTime.Now.AddYears(1);
                } else
                {
                    clanstvo.DatumIstek = korisnik.Clanstva.First().DatumIstek.AddYears(1);
                }
                }
                if (ModelState.IsValid)
                {
                if (korisnik.Clanstva.Count != 0)
                {
                    if (korisnik.Clanstva.Where(t => t.ClanstvoID == clanstvo.ClanstvoID).First() != null)
                    {
                        korisnik.Clanstva.Remove(korisnik.Clanstva.Where(t => t.ClanstvoID == clanstvo.ClanstvoID).First());
                    }
                } else
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    TempData["poruka"] = "Uspješno ste postali član udruženja. Ulogirajte se opet za potpuni pristup pogodnostima članstva!";
                }

                korisnik.Clanstva.Add(clanstvo);
                    
                db.SaveChanges();
                return RedirectToAction("Index", "Home", new { area = "" });
            }

                // TODO posalji obavijest o zaprimljenom clanstvu
                return RedirectToAction("Options");
  
        }

        public ActionResult Options()
        {
            ViewBag.role = User.IsInRole("Simpatizer");
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            ViewBag.isteklo = false;
            ViewBag.istek = null;
            if (korisnik.Clanstva!=null && korisnik.Clanstva.Count!=0)
            {
                var ist = korisnik.Clanstva.First().DatumIstek.CompareTo(DateTime.Now);
                ViewBag.isteklo = (ist <= 0) ? true : false;
                ViewBag.istek = korisnik.Clanstva.First().DatumIstek;
            }
            return View(korisnik);
        }

        [HttpPost]
        public ActionResult Options(FormCollection collection)
        {


            ViewBag.role = User.IsInRole("Simpatizer");
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            korisnik.Ime = Request.Form.Get("Ime");
            korisnik.Prezime = Request.Form.Get("Prezime");
            korisnik.Spol = Request.Form.Get("Spol");
            db.Entry(korisnik).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Options");
        }
        public ActionResult AllSub()
        {
            return View();
        }

    }
}
