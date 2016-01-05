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
    public class EpitesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new EpitesViewModel();

            using(var db = new ApplicationDbContext()){
                
                var userId = HttpContext.User.Identity.GetUserId();

                var orszag = await db.Orszagok
                    .Include("OrszagEpuletek.Epulet")
                    .Include("OrszagEpuletKeszulesek.Epulet")
                    .SingleOrDefaultAsync(o => o.User.Id == userId);

                var lehetsegesEpuletek = db.Epuletek.ToList();

                model.Epuletek = lehetsegesEpuletek.Select(e => new EpitesEpuletListViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Ido = e.Ido,
                    JelenlegVan = orszag.OrszagEpuletek.Where(oe => oe.Epulet.Id == e.Id).Sum(oe => oe.Darab),
                    Hatravan = orszag.OrszagEpuletKeszulesek.Where(oe => oe.Epulet.Id == e.Id).Sum(oe => oe.Hatravan)
                }).ToList();

                return View(model);


            }
        }

    }
}