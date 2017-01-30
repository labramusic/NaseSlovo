using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaseSlovoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.IsInRole("Simpatizer") && !User.IsInRole("Clan") && !User.IsInRole("Autor") && !User.IsInRole("Urednik") && !User.IsInRole("Lektor") && !User.IsInRole("GrafickiUrednik"))
            {
                if (TempData["poruka"] != null)
                {
                    ViewBag.poruka = TempData["poruka"].ToString();
                }
                return View();
            }
            return Redirect("Tekstovi/Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}