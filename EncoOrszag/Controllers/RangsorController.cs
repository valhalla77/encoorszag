using EncoOrszag.Models;
using EncoOrszag.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EncoOrszag.Models.DataAccess.Entities;

namespace EncoOrszag.Controllers
{
    public class RangsorController : Controller
    {
        
        public async Task<ActionResult> Index()
        {
            var model = new RangsorViewModel();

            using (var db = new ApplicationDbContext())
            {
                var orszagok = await db.Orszagok.OrderByDescending(o => o.Pontszam).ToListAsync();

                model.OrszagPontszam = orszagok.ToDictionary<Orszag, string, int>(
                k => k.Name,
                e =>
                {
                    var pontszam = orszagok.SingleOrDefault(s => s.Id == e.Id);
                    return pontszam == null ? 0 : pontszam.Pontszam;
                });
                return View(model);
            }
        }
    }
}