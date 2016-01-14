using EncoOrszag.Models;
using EncoOrszag.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using EncoOrszag.Models.DataAccess.Entities;

namespace EncoOrszag.Controllers
{
   [Authorize]
   public class HaboruController : Controller
   {
      public async Task<ActionResult> Index()
      {
         var model = new HaboruViewModel();

         using (var db = new ApplicationDbContext())
         {
            var userId = HttpContext.User.Identity.GetUserId();
            var orszag = await db.Orszagok
               .Include(o => o.User)
               .Include(o => o.OrszagEgysegek.Select(oe => oe.Egyseg))
               .Include(o => o.SajatHadseregek.Select(sh => sh.HadseregEgysegek.Select(he => he.Egyseg)))
               .Include(o => o.SajatHadseregek.Select(sh => sh.CelOrszag))
               .Include(o => o.OrszagFejlesztesek.Select(of => of.Fejlesztes))
               .SingleOrDefaultAsync(o => o.User.Id == userId);

            var lehetsegesEgysegek = db.Egysegek.ToList();

            var osszesorszag = db.Orszagok.ToList();

            model.Orszagok = osszesorszag.Where(o => o.Id != orszag.Id).Select(e => new SelectListItem
            {
               Text = e.Name,
               Value = e.Id.ToString()
            }).ToList();


            model.JelenlegiEgysegek = lehetsegesEgysegek.Select(e => new HaboruEgysegListViewModel
            {
               Id = e.Id,
               Name = e.Name,
               Tamadas = e.Tamadas,
               Vedekezes = e.Vedekezes,
               OsszesenVan = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Id == e.Id).Sum(oe => oe.Darab)
            }).ToList();

            model.Hadseregek = orszag.SajatHadseregek.Select(sh => new HaboruHadseregListViewModel
            {
               Id = sh.Id,
               CelOrszag = sh.CelOrszag.Name,
               CelOrszagId = sh.CelOrszag.Id,
               HadseregEgysegek = sh.HadseregEgysegek.Select(he => new HadseregEgysegListViewModel
               {

                  Id = he.Egyseg.Id,
                  Egyseg = he.Egyseg.Name,
                  Darab = he.Darab
               }).ToList()
            }).ToList();

            foreach (var item in model.JelenlegiEgysegek)
            {
               var hadseregben = model.Hadseregek.Sum(h => h.HadseregEgysegek.SingleOrDefault(he => he.Egyseg == item.Name).Darab);
               item.JelenlegVan = item.OsszesenVan - hadseregben;
            }
         }


         return View(model);
      }

      [HttpPost]
     
      public async Task<ActionResult> Ujhadsereg(HaboruHadseregListViewModel model)
      {
         using (var db = new ApplicationDbContext())
         {

            var userId = HttpContext.User.Identity.GetUserId();
            var orszag = await db.Orszagok
              .Include(o => o.User)
              .Include(o => o.OrszagEgysegek.Select(oe => oe.Egyseg))
              .Include(o => o.SajatHadseregek.Select(sh => sh.HadseregEgysegek.Select(he => he.Egyseg)))
              .Include(o => o.SajatHadseregek.Select(sh => sh.CelOrszag))
              .SingleOrDefaultAsync(o => o.User.Id == userId);

            var lehetsegesEgysegek = db.Egysegek.ToList();

            var jelenlegiEgysegek = lehetsegesEgysegek.Select(e => new HaboruEgysegListViewModel
            {
               Id = e.Id,
               Name = e.Name,
               Tamadas = e.Tamadas,
               Vedekezes = e.Vedekezes,
               OsszesenVan = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Id == e.Id).Sum(oe => oe.Darab)
            }).ToList();

            foreach (var item in jelenlegiEgysegek)
            {
               var hadseregben = orszag.SajatHadseregek.Sum(sh => sh.HadseregEgysegek.SingleOrDefault(he => he.Egyseg.Id == item.Id).Darab);
               item.JelenlegVan = item.OsszesenVan - hadseregben;
            }


            if (db.Orszagok.Any(o => o.Id == model.CelOrszagId))
            {

               foreach (var item in model.HadseregEgysegek)
               {
                  if (jelenlegiEgysegek.Single(je => je.Id == item.Id).JelenlegVan < item.Darab)
                  {

                     ModelState.AddModelError("", "Nincs elég egység!");
                  }
               }
            }
            else
            {
               ModelState.AddModelError("", "Nincs ilyen ország!");
            }
            if (ModelState.IsValid)
            {
               var ujhadsereg = new Hadsereg
               {
                  CelOrszag = db.Orszagok.SingleOrDefault(o => o.Id == model.CelOrszagId),
                  HadseregEgysegek = model.HadseregEgysegek.Select(he => new HadseregEgyseg
                  {
                     Egyseg = lehetsegesEgysegek.Single(le => le.Id == he.Id),
                     Darab = he.Darab
                  }).ToList()
               };
               orszag.SajatHadseregek.Add(ujhadsereg);

               await db.SaveChangesAsync();
               model.Id = ujhadsereg.Id;
               model.CelOrszag = ujhadsereg.CelOrszag.Name;

               return Json(new
               {
                  Success = true,
                  UjHadsereg = model
               });

            }
            else
            {
               return Json(new
               {
                  Success = false
               });
            }


         }
      }
   }
}