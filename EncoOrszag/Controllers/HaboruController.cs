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

namespace EncoOrszag.Controllers
{
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

            model.Orszagok = osszesorszag.Select(e => new SelectListItem
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

                  Id = he.Id,
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
      [ValidateAntiForgeryToken]
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

            if (db.Orszagok.Any(o => o.Id == model.CelOrszagId))
            {
               foreach (var item in model.HadseregEgysegek)
               {
                  ////ittartok
               }
            }

            //ellenorzes model
            //mentes
            //visszaadas, hadsereg+success vagy error

            return Json(new
            {
               Success = true,
               UjHadsereg = model
            });


            return Json(new
            {
               Success = false
            });

         }
      }
   }
}