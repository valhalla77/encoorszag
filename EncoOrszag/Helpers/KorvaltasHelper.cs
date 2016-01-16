using EncoOrszag.Models.DataAccess;
using EncoOrszag.Models.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Runtime.InteropServices;

namespace EncoOrszag.Helpers
{
    public class KorvaltasHelper : IDisposable
    {
        private ApplicationDbContext db;

        public KorvaltasHelper()
        {
            this.db = new ApplicationDbContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null) db.Dispose();
            }
        }

        public void Korvaltas()
        {

            var orszagok = db.Orszagok
            .Include(o => o.User)

            .Include(o => o.OrszagEpuletek.Select(oe => oe.Epulet))
            .Include("OrszagEpuletKeszulesek.Epulet")
            .Include("OrszagEgysegek.Egyseg")
            .Include("OrszagFejlesztesek.Fejlesztes")
            .Include("OrszagFejlesztesKeszulesek.Fejlesztes")
            //.Include(o => o.SajatHadseregek.Select(sh => sh.HadseregEgysegek.Select(he => he.Egyseg)))
            .ToList();




            foreach (var orszag in orszagok)
            {
                Adozas(orszag);
                Krumpli(orszag);
                Zsold(orszag);
                Ellatmany(orszag);

            }

            EpitesKeszul();
            FejlesztesKeszul();

            foreach (var hadsereg in hadseregek)
            {
                var celorszag = orszagok.Single(o => o == hadsereg.CelOrszag);
                var sajatorszag = orszagok.Single(o => o == hadsereg.SajatOrszag);
                Harc(hadsereg, celorszag, sajatorszag, lehetsegesEgysegek);
            }

            foreach (var orszag in orszagok)
            {
                Pontszam(orszag);
            }
            db.SaveChanges();


        }

        private void Adozas(Orszag orszag)
        {
            var tanya = orszag.OrszagEpuletek.SingleOrDefault(o => o.Epulet.Name == "Tanya");
            var alkimia = orszag.OrszagFejlesztesek.SingleOrDefault(o => o.Fejlesztes.Name == "Alkímia");

            if (alkimia != null)
            {
                //1 tanya: 50 ember, 1 ember:  25 arany /kör, alkimia 30%-al több arany
                orszag.Arany += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 50 * 25 * 1.3);
            }
            else
            {
                orszag.Arany += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 50 * 25);
            }
        }

        private void Krumpli(Orszag orszag)
        {
            var tanya = orszag.OrszagEpuletek.SingleOrDefault(o => o.Epulet.Name == "Tanya");
            var traktor = orszag.OrszagFejlesztesek.SingleOrDefault(o => o.Fejlesztes.Name == "Traktor");
            var kombajn = orszag.OrszagFejlesztesek.SingleOrDefault(o => o.Fejlesztes.Name == "Kombájn");

            //1 tanya: 200 krumpli/kör, traktor 10%-al több krumpli, kombájn 15%-al több
            var szorzo = 1.0;


            if (traktor != null)
            {
                szorzo = szorzo * 1.1;
            }

            if (kombajn != null)
            {
                szorzo = szorzo * 1.15;
            }
            orszag.Krumpli += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 200 * szorzo);


        }

        private void Zsold(Orszag orszag)
        {
            var zsold = 0;
            foreach (var item in orszag.OrszagEgysegek)
            {
                zsold += item.Darab * item.Egyseg.Zsold;
            }


            orszag.Arany -= zsold;
        }

        private void Ellatmany(Orszag orszag)
        {
            var ellatmany = 0;
            foreach (var item in orszag.OrszagEgysegek)
            {
                ellatmany += item.Darab * item.Egyseg.Ellatmany;
            }

            orszag.Krumpli -= ellatmany;
        }

        private void EpitesKeszul()
        {
            var epuletek = db.OrszagEpuletKeszulesek
               //.Include("Orszag.OrszagEpuletek.Epulet")
               .Include(oek => oek.Orszag.OrszagEpuletek.Select(oe => oe.Epulet))
               .Include(e => e.Epulet)
               .ToList();

            foreach (var item in epuletek)
            {
                item.Hatravan--;
                if (item.Hatravan == 0)
                {
                    var orszagepulet = item.Orszag.OrszagEpuletek.SingleOrDefault(oe => oe.Epulet.Id == item.Epulet.Id);

                    if (orszagepulet == null)
                    {
                        db.OrszagEpuletek.Add(new OrszagEpulet
                        {
                            Orszag = item.Orszag,
                            Epulet = item.Epulet,
                            Darab = 1
                        });
                    }
                    else
                    {
                        orszagepulet.Darab++;
                    }
                }
            }

            db.OrszagEpuletKeszulesek.RemoveRange(db.OrszagEpuletKeszulesek.Where(e => e.Hatravan == 0));
        }

        private void FejlesztesKeszul()
        {
            var fejlesztesek = db.OrszagFejlesztesKeszulesek
               .Include("Orszag.OrszagFejlesztesek.Fejlesztes")
               .Include(e => e.Fejlesztes)
               .ToList();

            foreach (var item in fejlesztesek)
            {
                item.Hatravan--;
                if (item.Hatravan == 0)
                {
                    var orszagfejlesztes = item.Orszag.OrszagFejlesztesek.SingleOrDefault(oe => oe.Fejlesztes.Id == item.Fejlesztes.Id);

                    if (orszagfejlesztes == null)
                    {
                        db.OrszagFejlesztesek.Add(new OrszagFejlesztes
                        {
                            Orszag = item.Orszag,
                            Fejlesztes = item.Fejlesztes,

                        });
                    }

                }
            }

            db.OrszagFejlesztesKeszulesek.RemoveRange(db.OrszagFejlesztesKeszulesek.Where(e => e.Hatravan == 0));
        }

        private void Harc()
        {

            var hadseregek = db.Hadseregek
               .Include(h => h.HadseregEgysegek.Select(he => he.Egyseg))
               .Include(h => h.CelOrszag.OrszagEgysegek.Select(oe => oe.Egyseg))
               .Include(h => h.CelOrszag.User)
               .Include(h => h.CelOrszag.OrszagFejlesztesek.Select(of => of.Fejlesztes))
               .Include(h => h.SajatOrszag.OrszagEgysegek.Select(oe => oe.Egyseg))
               .Include(h => h.SajatOrszag.User)
               .Include(h => h.SajatOrszag.OrszagFejlesztesek.Select(of => of.Fejlesztes))
               .ToList();

            var lehetsegesEgysegek = db.Egysegek.ToList();

            foreach (var hadsereg in hadseregek)
            {
                var celorszag = hadsereg.CelOrszag;
                var sajatorszag = hadsereg.SajatOrszag;
                var gyozelem = Csata(hadsereg, celorszag, sajatorszag, lehetsegesEgysegek);
            }

        }

        private bool Csata(Hadsereg hadsereg, Orszag celorszag, Orszag sajatorszag, List<Egyseg> lehetsegesEgysegek)
        {
            var tamadoertek = 0;
            var vedoertek = 0;

            var tamadoszorzo = 1.0;
            var vedoszorzo = 1.0;


            //fejlesztések
            //sajatorzsag
            if (sajatorszag.OrszagFejlesztesek.Any(of => of.Fejlesztes.Id == 4)) //operaton rebirth
            {
                tamadoszorzo *= 1.2;
            }
            if (sajatorszag.OrszagFejlesztesek.Any(of => of.Fejlesztes.Id == 5)) //taktika
            {
                tamadoszorzo *= 1.1;
            }

            //celorszag
            if (celorszag.OrszagFejlesztesek.Any(of => of.Fejlesztes.Id == 3)) //operaton rebirth
            {
                vedoszorzo *= 1.2;
            }
            if (celorszag.OrszagFejlesztesek.Any(of => of.Fejlesztes.Id == 5)) //taktika
            {
                vedoszorzo *= 1.1;
            }


            //a hadseregek tamadoertekenek szamitasa
            foreach (var egyseg in hadsereg.HadseregEgysegek)
            {
                tamadoertek += egyseg.Egyseg.Tamadas * egyseg.Darab;
            }

            tamadoertek = Convert.ToInt32(tamadoertek * tamadoszorzo);
            //celorszag vedoerteke
            var vedoegysegek = lehetsegesEgysegek.Select(e => new
            {

                Vedekezes = e.Vedekezes,
                JelenlegVan =
               celorszag.OrszagEgysegek.Where(oe => oe.Egyseg.Id == e.Id).Sum(oe => oe.Darab)  //összesegység
               - celorszag.SajatHadseregek.Sum(h => h.HadseregEgysegek.SingleOrDefault(he => he.Egyseg.Id == e.Id).Darab) //jelenleg hadseregben, tehát nem véd

            });

            foreach (var item in vedoegysegek)
            {
                vedoertek += item.JelenlegVan * item.Vedekezes;
            }

            vedoertek = Convert.ToInt32(vedoertek * vedoszorzo);

            var gyozelem = new bool();
            ////harc
            //győzelem
            if (tamadoertek > vedoertek)
            {
                gyozelem = true;
            }
            else //vereség
            {
                gyozelem = false;
            }
            return gyozelem;


        }

        private void Pontszam(Orszag orszag)
        {
            var tanya = orszag.OrszagEpuletek.SingleOrDefault(oe => oe.Epulet.Name == "Tanya");

            var tanyaszam = tanya == null ? 0 : tanya.Darab;

            var barakk = orszag.OrszagEpuletek.SingleOrDefault(oe => oe.Epulet.Name == "Barakk");

            var barakkszam = barakk == null ? 0 : barakk.Darab;

            var lovasijasz = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Name == "Lovas" || oe.Egyseg.Name == "Íjász").Sum(o => o.Darab);

            var elit = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Name == "Elit").Sum(o => o.Darab);

            var tudomany = orszag.OrszagFejlesztesek.Count();

            orszag.Pontszam =
                 tanyaszam * 50 // emberek száma*1
               + (tanyaszam + barakkszam) * 50 //épületek száma*50
               + lovasijasz * 5 //lovas, ijasz darab* 5
               + tudomany * 100 //fejlesztes * 100
               ;
        }


    }
}