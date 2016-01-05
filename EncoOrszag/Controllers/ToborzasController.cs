using EncoOrszag.Models;
using EncoOrszag.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using EncoOrszag.Models.DataAccess.Entities;

namespace EncoOrszag.Controllers
{
    public class ToborzasController : Controller
    {

        public async Task<ActionResult> Index()
        {
            var model = new ToborzasViewModel();

            using (var db = new ApplicationDbContext())
            {
                var userId = HttpContext.User.Identity.GetUserId();
                var orszag = await db.Orszagok
                    .Include("OrszagEgysegek.Egyseg")
                    .Include("OrszagEpuletek.Epulet")
                    .SingleOrDefaultAsync(o => o.User.Id == userId);

                var lehetsegesEgysegek = db.Egysegek.ToList();

                model.Arany = orszag.Arany;

                var barakk = orszag.OrszagEpuletek.SingleOrDefault(o => o.Epulet.Name == "Barakk");
                var osszeshely = barakk == null ? 0 : barakk.Darab * 200;





                model.Egysegek = lehetsegesEgysegek.Select(e => new ToborzasEgysegListViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Tamadas = e.Tamadas,
                        Vedekezes = e.Vedekezes,
                        Ar = e.Ar,
                        Zsold = e.Zsold,
                        Ellatmany = e.Ellatmany,

                        JelenlegVan = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Id == e.Id).Sum(oe => oe.Darab)

                    }).ToList();

                model.Szabadhelyek = osszeshely - model.Egysegek.Sum(e => e.JelenlegVan);


            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ToborzasViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var userId = HttpContext.User.Identity.GetUserId();
                var orszag = await db.Orszagok
                    .Include(o => o.User)       //ez nem kéne, hanem az entitystate-tel kéne megoldani, csak nincs kedvem most
                    .Include("OrszagEgysegek.Egyseg")
                    .Include("OrszagEpuletek.Epulet")
                    .SingleOrDefaultAsync(o => o.User.Id == userId);

                var lehetsegesEgysegek = db.Egysegek.ToList();

                // ezt kéne jól csinálni a user include-ja helyett
                db.Entry<Orszag>(orszag).Reference(o => o.User).EntityEntry.State = EntityState.Unchanged;

                var barakk = orszag.OrszagEpuletek.SingleOrDefault(o => o.Epulet.Name == "Barakk");
                var osszeshely = barakk == null ? 0 : barakk.Darab * 200;

                var szabadhely = osszeshely - orszag.OrszagEgysegek.Sum(e => e.Darab);

                var vetel = 0;
                var vetelar = 0; 
                foreach (var item in model.Egysegek)
                {
                    vetel += (item.Vetel.HasValue ? item.Vetel.Value : 0);
                    vetelar += (item.Vetel.HasValue ? item.Vetel.Value*item.Ar : 0);
                }

                if (vetel > szabadhely)
                {
                    ModelState.AddModelError("", "Nincs szabad helyed!");
                }

                if (vetelar > orszag.Arany)
                {
                    ModelState.AddModelError("", "Nincs elég aranyad!");
                }

                if (ModelState.IsValid)
                {
                    foreach (var item in model.Egysegek)
                    {
                        var egyseg = orszag.OrszagEgysegek.SingleOrDefault(e => e.Egyseg.Id == item.Id);
                        if (egyseg == null)
                        {
                            orszag.OrszagEgysegek.Add(new Models.DataAccess.Entities.OrszagEgyseg
                            {
                                Orszag = orszag,
                                Egyseg = lehetsegesEgysegek.Single(e => e.Id == item.Id),
                                Darab = item.Vetel.HasValue ? item.Vetel.Value : 0
                            });
                        }
                        else
                        {
                            egyseg.Darab += item.Vetel.HasValue ?  item.Vetel.Value : 0;
                        }
                        
                    }

                    orszag.Arany -= vetelar;


                    await db.SaveChangesAsync();


                    return RedirectToAction("Index");
                }

                return View(model);


            } 
        }

    }


}