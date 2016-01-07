using EncoOrszag.Models;
using EncoOrszag.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using EncoOrszag.Models.DataAccess.Entities;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using EncoOrszag.Helpers;

namespace EncoOrszag.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {

      public async Task<ActionResult> Index()
      {
         var model = new OrszagOverviewViewModel();

         using (var db = new ApplicationDbContext())
         {
            var userId = HttpContext.User.Identity.GetUserId();
            var orszag = await db.Orszagok
                .Include("OrszagEpuletek.Epulet")
                .Include("OrszagEpuletKeszulesek.Epulet")
                .Include("OrszagFejlesztesek.Fejlesztes")
                .Include("OrszagFejlesztesKeszulesek.Fejlesztes")
                .Include("OrszagEgysegek.Egyseg")
                .SingleOrDefaultAsync(o => o.User.Id == userId);

            var lehetsegesEgysegek = db.Egysegek.ToList();
            var lehetsegesEpuletek = db.Epuletek.ToList();
            var lehetsegesFejlesztesek = db.Fejlesztesek.ToList();

            model.Name = orszag.Name;
            model.Pontszam = orszag.Pontszam;
            model.Krumpli = orszag.Krumpli;
            model.Arany = orszag.Arany;

            var epuletTanya = orszag.OrszagEpuletek.SingleOrDefault(o => o.Epulet.Name == "Tanya");
            model.Nepesseg = epuletTanya == null ? 0 : epuletTanya.Darab * 50;

            model.Egysegek = lehetsegesEgysegek.ToDictionary<Egyseg, string, int>(
                k => k.Name,
                e =>
                {
                   var egyseg = orszag.OrszagEgysegek.SingleOrDefault(s => s.Egyseg.Id == e.Id);
                   return egyseg == null ? 0 : egyseg.Darab;
                });

            model.Epuletek = lehetsegesEpuletek.ToDictionary<Epulet, string, int>(
                k => k.Name,
                e =>
                {
                   var epulet = orszag.OrszagEpuletek.SingleOrDefault(s => s.Epulet.Id == e.Id);
                   return epulet == null ? 0 : epulet.Darab;
                });

            model.Fejlesztesek = lehetsegesFejlesztesek.ToDictionary<Fejlesztes, string, bool>(
                k => k.Name,
                e =>
                {
                   var fejlesztes = orszag.OrszagFejlesztesek.SingleOrDefault(s => s.Fejlesztes.Id == e.Id);
                   return fejlesztes != null;
                });

            model.Epitesalatt = orszag.OrszagEpuletKeszulesek.ToDictionary<OrszagEpuletKeszul, string, int>(
                k => k.Epulet.Name,
                e => e.Hatravan
                );

            model.Felesztesalatt = orszag.OrszagFejlesztesKeszulesek.ToDictionary<OrszagFejlesztesKeszul, string, int>(
                k => k.Fejlesztes.Name,
                e => e.Hatravan
                );

            //model.SzabadHelyek = model.Epuletek["Barakk"]*200 - model.Egysegek["Lovas"] - model.Egysegek["Íjász"] - model.Egysegek["Elit"];


         }


         return View(model);
      }

      public ActionResult Korvaltas(string returnUrl)
      {
        
         KorvaltasHelper.Korvaltas();
         return RedirectToLocal(returnUrl);
      }

      private ActionResult RedirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return Redirect(returnUrl);
         }
         return RedirectToAction("Index", "Home");
      }

   }


}
