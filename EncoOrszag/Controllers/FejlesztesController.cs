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
    [Authorize]
    public class FejlesztesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new FejlesztesViewModel();

            using (var db = new ApplicationDbContext())
            {

                var userId = HttpContext.User.Identity.GetUserId();

                var orszag = await db.Orszagok
                    .Include("OrszagFejlesztesek.Fejlesztes")
                    .Include("OrszagFejlesztesKeszulesek.Fejlesztes")
                    .SingleOrDefaultAsync(o => o.User.Id == userId);

                var lehetsegesFejlesztesek = db.Fejlesztesek.ToList();

                model.Fejlesztesek = lehetsegesFejlesztesek.Select(f => new FejlesztesFejlesztesListViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Ido = f.Ido,
                    Kifejlesztve = orszag.OrszagFejlesztesek.Any(of => of.Fejlesztes.Id == f.Id),
                    Hatravan = orszag.OrszagFejlesztesKeszulesek.Where(of => of.Fejlesztes.Id == f.Id).Sum(of => of.Hatravan)
                }).ToList();

                return View(model);


            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(FejlesztesViewModel model)
        {


            using (var db = new ApplicationDbContext())
            {

                var userId = HttpContext.User.Identity.GetUserId();

                var orszag = await db.Orszagok
                    .Include("OrszagFejlesztesek.Fejlesztes")
                    .Include("OrszagFejlesztesKeszulesek.Fejlesztes")
                    .SingleOrDefaultAsync(o => o.User.Id == userId);

                var lehetsegesFejlesztesek = db.Fejlesztesek.ToList();
                
                var jelenlegfejleszt = orszag.OrszagFejlesztesKeszulesek.Count();

                var fejleszt = model.Fejleszt;

                if (jelenlegfejleszt != 0)
                {
                    ModelState.AddModelError("", "Egyszerre csak egy technológia fejleszthető!");
                }

                if (fejleszt == 0)
                {
                    ModelState.AddModelError("", "Nem választottál fejlesztést!");
                }

                if (ModelState.IsValid)
                {
                    orszag.OrszagFejlesztesKeszulesek.Add(new Models.DataAccess.Entities.OrszagFejlesztesKeszul
                    {
                        Orszag = orszag,
                        Fejlesztes = lehetsegesFejlesztesek.Single(e => e.Id == fejleszt),
                        Hatravan = lehetsegesFejlesztesek.Single(e => e.Id == fejleszt).Ido
                    });

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");

                }
                else
                {
                    return View(model);
                }

            }
        }
    }
}