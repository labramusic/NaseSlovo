using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NaseSlovoApp.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System.IO;

namespace NaseSlovoApp.Controllers
{

    public class TekstoviController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public TekstoviController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: Tekstovi
        public ActionResult Index(int? page)
        {
            // TODO Roles.GetRolesForUser ne radi
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            if (!User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
            {
                tekstovi = tekstovi.Where(t => !t.Privatan);
            }
            tekstovi = tekstovi.Where(t => t.Objavljen)
                .OrderByDescending(t => t.DatumVrijeme);

            //int pageSize = (int)Math.Ceiling((double)tekstovi.Count() / 10);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(string searchString, int? page)
        {
            var tekstovi = from t in db.Tekstovi
                           select t;
            // ili samo db.tekstovi ?
            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
                // pretrazivanje po naslovu
                tekstovi = tekstovi
                    .Where(s => s.Naslov.Contains(searchString))
                    .Where(t => t.Objavljen);

                if (!User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
                {
                    tekstovi = tekstovi.Where(t => !t.Privatan);
                }
            }

            ViewBag.SearchString = searchString;

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.OrderByDescending(t => t.DatumVrijeme).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Top(int? page)
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            // rangiraj po srednjoj ocjeni i odaberi prvih 10
            tekstovi = tekstovi
                .Where(t => t.Objavljen)
                .OrderByDescending(t => t.SrednjaOcjena)
                .Take(10);

            if (!User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
            {
                tekstovi = tekstovi.Where(t => !t.Privatan)
                    .OrderByDescending(t => t.SrednjaOcjena);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Neobjavljeno(int? page)
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            // rangiraj po srednjoj ocjeni i odaberi prvih 10
            tekstovi = tekstovi
                .Where(t => t.Objavljen == false)
                .OrderByDescending(t => t.DatumVrijeme);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Pretplate(int? page)
        {
            string id = User.Identity.GetUserId();
            ApplicationUser user = ApplicationDbContext.Users.Find(id);
            Korisnik korisnik = db.Korisnici.Find(user.KorisnikID);
            var tekstovi = db.Tekstovi;
            var autori = korisnik.Autori;
            var vrste = korisnik.Vrste;
            var teme = korisnik.Teme;
            List<Tekst> tekst = new List<Tekst>();
            foreach (Tekst item in tekstovi)
            {
                if ((vrste.Contains(item.Vrsta) || teme.Contains(item.Tema) || autori.Contains(item.Korisnik)) && item.Objavljen)
                {
                    tekst.Add(item);
                }
            }
            //tekst.Sort();

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekst.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var path = Path.Combine(Server.MapPath("~/Content/Files/"), file.FileName);

            var data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);
            using (var sw = new FileStream(path, FileMode.Create))
            {
                sw.Write(data, 0, data.Length);

            }
            var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/Files/" + file.FileName));
            TempData["tekst"] = fileContents.ToString();
            return RedirectToAction("Create");
        }

        // GET
        public ActionResult UrediBilten()
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            // rangiraj po srednjoj ocjeni i odaberi prvih 10
            tekstovi = tekstovi
                .Where(t => t.Objavljen)
                .Where(t => !t.Privatan)
                .Where(t => t.DatumVrijeme.Month == DateTime.Now.Month
                && t.DatumVrijeme.Year == DateTime.Now.Year)
                .OrderByDescending(t => t.SrednjaOcjena)
                .Take(10)
                .Where(t => !t.Odabran);
            
            return View("UrediBilten", tekstovi.ToList());
        }

        // POST
        [HttpPost]
        public ActionResult UrediBilten(List<Tekst> nista)
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            // rangiraj po srednjoj ocjeni i odaberi prvih 10
            tekstovi = tekstovi
                .Where(t => t.Objavljen)
                .Where(t => !t.Privatan)
                .Where(t => t.DatumVrijeme.Month == DateTime.Now.Month
                && t.DatumVrijeme.Year == DateTime.Now.Year)
                .OrderByDescending(t => t.SrednjaOcjena)
                .Take(10);

            foreach (Tekst tekst in tekstovi)
            {
                tekst.Odabran = true;
            }
            db.SaveChanges();

            return View("UrediBilten", tekstovi.ToList());
        }

        // GET
        public ActionResult UrediBiltenOdabrani(int? page)
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            tekstovi = tekstovi
                .Where(t => t.Objavljen)
                .Where(t => !t.Privatan)
                .Where(t => t.Odabran)
                .Where(t => t.DatumVrijeme.Month == DateTime.Now.Month
                && t.DatumVrijeme.Year == DateTime.Now.Year)
                .OrderByDescending(t => t.SrednjaOcjena);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Bilten()
        {
            // ako ima u bazi s trenutnim mjesecom i godinom
            if (db.Bilteni.Where(b => b.Datum.Month == DateTime.Now.Month
            && b.Datum.Year == DateTime.Now.Year).Count() > 0)
            {
                // 
                string datum = DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                var srcFile = Server.MapPath("~/Content/Files/bilten" + datum + ".pdf");
                //string contents = System.IO.File.ReadAllText(srcFile);
                var destFile = Server.MapPath("~/App_Data/bilten" + datum + ".pdf");
                //System.IO.File.WriteAllText(destFile, contents);
                System.IO.File.Copy(srcFile, destFile);
            }
            
            // preuzmi
            return RedirectToAction("Index");
        }

        public ActionResult Zbornik()
        {
            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            tekstovi = tekstovi
                .Where(t => t.Objavljen)
                .Where(t => !t.Privatan)
                .Where(t => t.DatumVrijeme.Year == DateTime.Now.Year);

            var zbornikTekstovi = new List<Tekst>();
            for (int month=1; month<=12; ++month)
            {
                var top = tekstovi
                    .Where(t => t.DatumVrijeme.Month == month)
                    .OrderByDescending(t => t.SrednjaOcjena);
                if (top.Count() == 0) continue;
                var tekst = top.Take(1).First();
                zbornikTekstovi.Add(tekst);
            }

            return View(zbornikTekstovi);
        }

        // GET
        public ActionResult PostaviBilten()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostaviBilten(HttpPostedFileBase file)
        {
            var path = Path.Combine(Server.MapPath("~/Content/Files/"), file.FileName);

            var data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);
            using (var sw = new FileStream(path, FileMode.Create))
            {
                sw.Write(data, 0, data.Length);

            }
            return View();
        }

        // GET
        public ActionResult PostaviBiltenUrednik()
        {
            Bilten bilten = new Bilten();
            bilten.Datum = DateTime.Now;
            db.Bilteni.Add(bilten);
            //dodaj tekstove u bazu (ne)
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // premjesteno u index
        public ActionResult All(int? page)
        {
            // TODO Roles.GetRolesForUser ne radi
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var result1 = UserManager.AddToRole(User.Identity.GetUserId(), "Simpatizer");
            var roles = UserManager.GetRoles(User.Identity.GetUserId());

            var tekstovi = db.Tekstovi.Include(t => t.Korisnik).Include(t => t.Tema).Include(t => t.Vrsta);
            if (!User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
            {
                tekstovi = tekstovi.Where(c => c.Privatan == false)
                    .OrderByDescending(t => t.DatumVrijeme);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("All", tekstovi.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tekstovi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }         
            Tekst tekst = db.Tekstovi.Find(id);
            bool hasRight = true;
            if (tekst == null)
            {
                return HttpNotFound();
            }
            string decision = Request.Form["decision"];
            string privatan = Request.Form["privatan"];
            if(privatan == "privatan" && !tekst.Privatan)
            {
                tekst.Privatan = true;
                db.SaveChanges();
                return RedirectToAction("Neobjavljeno");
            } else if (privatan=="javan" && tekst.Privatan)
            {
                tekst.Privatan = false;
                db.SaveChanges();
                return RedirectToAction("Neobjavljeno");
            }
            if (decision == "Prihvati")
            {
                tekst.Objavljen = true;
                db.SaveChanges();
                return RedirectToAction("Neobjavljeno");
            }
            else if (decision == "Odbij")
            {
                db.Tekstovi.Remove(tekst);
                db.SaveChanges();
                return RedirectToAction("Neobjavljeno");
            }
            else
            {
                if (!User.IsInRole("Simpatizer") && !User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
                {
                    hasRight = false;
                }
            }
            ViewData["HasRight"] = hasRight;
            ViewData["Objavljen"] = tekst.Objavljen;
            ViewData["Privatan"] = tekst.Privatan;
            return View(tekst);
        }
        
        // GET: Tekstovi/Create
        public ActionResult Create()
        {
            ViewBag.BiltenID = new SelectList(db.Bilteni, "BiltenID", "BiltenID");
            ViewBag.AutorID = new SelectList(db.Korisnici, "KorisnikID", "Ime");
            ViewBag.TemaID = new SelectList(db.Teme, "TemaID", "Naziv");
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv");
            return View();
        }

        // POST: Tekstovi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TekstID,Sadrzaj,Naslov,TemaID,VrstaID")] Tekst tekst)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                ApplicationUser user = ApplicationDbContext.Users.Find(id);

                tekst.AutorID = user.KorisnikID;
                tekst.DatumVrijeme = DateTime.Now;
                tekst.SrednjaOcjena = 0;
                db.Tekstovi.Add(tekst);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BiltenID = new SelectList(db.Bilteni, "BiltenID", "BiltenID", tekst.BiltenID);
            ViewBag.AutorID = new SelectList(db.Korisnici, "KorisnikID", "Ime", tekst.AutorID);
            ViewBag.TemaID = new SelectList(db.Teme, "TemaID", "Naziv", tekst.TemaID);
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", tekst.VrstaID);
            return View(tekst);
        }

        // GET: Tekstovi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tekst tekst = db.Tekstovi.Find(id);
            if (tekst == null)
            {
                return HttpNotFound();
            }
            ViewBag.BiltenID = new SelectList(db.Bilteni, "BiltenID", "BiltenID", tekst.BiltenID);
            ViewBag.AutorID = new SelectList(db.Korisnici, "KorisnikID", "Ime", tekst.AutorID);
            ViewBag.TemaID = new SelectList(db.Teme, "TemaID", "Naziv", tekst.TemaID);
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", tekst.VrstaID);
            return View(tekst);
        }

        // POST: Tekstovi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TekstID,Sadrzaj,Naslov,AutorID,TemaID,VrstaID,DatumVrijeme,SrednjaOcjena,BiltenID")] Tekst tekst)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tekst).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BiltenID = new SelectList(db.Bilteni, "BiltenID", "BiltenID", tekst.BiltenID);
            ViewBag.AutorID = new SelectList(db.Korisnici, "KorisnikID", "Ime", tekst.AutorID);
            ViewBag.TemaID = new SelectList(db.Teme, "TemaID", "Naziv", tekst.TemaID);
            ViewBag.VrstaID = new SelectList(db.Vrste, "VrstaID", "Naziv", tekst.VrstaID);
            return View(tekst);
        }

        // GET: Tekstovi/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tekst tekst = db.Tekstovi.Find(id);
            if (tekst == null)
            {
                return HttpNotFound();
            }
            return View(tekst);
        }

        // POST: Tekstovi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tekst tekst = db.Tekstovi.Find(id);
            if (tekst == null)
            {
                return HttpNotFound();
            }
            db.Tekstovi.Remove(tekst);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Del()
        {
            int id = Int32.Parse(Request.Form.Get("TekstID"));
            Tekst tekst = db.Tekstovi.Find(id);
            if (ModelState.IsValid)
            {
                db.Tekstovi.Remove(tekst);
                db.SaveChanges();
            }
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