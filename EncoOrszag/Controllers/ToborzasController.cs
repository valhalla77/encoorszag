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

    }
}