using EncoOrszag.Models.DataAccess;
using EncoOrszag.Models.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EncoOrszag.Helpers
{
   public class KorvaltasHelper
   {
      public static void Korvaltas()
      {

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
         if (traktor != null)
         {
            if (kombajn != null)
            {
               orszag.Krumpli += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 200 * 1.15 * 1.10);
            }
            else
            {
               orszag.Krumpli += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 200 * 1.10);
            }
         }
         else
         {
            if (kombajn != null)
            {
               orszag.Krumpli += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 200 * 1.15);
            }
            else
            {
               orszag.Krumpli += Convert.ToInt32((tanya == null ? 0 : tanya.Darab) * 200);
            }
         }
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

      private void EpitesKeszul(ApplicationDbContext db)
      {
         var epuletek = db.OrszagEpuletKeszulesek
            .Include("Orszag.OrszagEpuletek.Epulet")
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

      private void FejlesztesKeszul(ApplicationDbContext db)
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

      private void Pontszam(Orszag orszag)
      {
         var tanya = orszag.OrszagEpuletek.SingleOrDefault(oe => oe.Epulet.Name == "Tanya");

         var barakk = orszag.OrszagEpuletek.SingleOrDefault(oe => oe.Epulet.Name == "Barakk");

         var lovasijasz = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Name == "Lovas" || oe.Egyseg.Name == "Íjász").Sum(o => o.Darab);

         var elit = orszag.OrszagEgysegek.Where(oe => oe.Egyseg.Name == "Elit").Sum(o => o.Darab);

         var tudomany = orszag.OrszagFejlesztesek.Count();

         orszag.Pontszam =
              tanya.Darab * 50 // emberek száma*1
            + (tanya.Darab + barakk.Darab) * 50 //épületek száma*50
            + lovasijasz * 5 //lovas, ijasz darab* 5
            + tudomany * 100 //fejlesztes * 100
            ;
      }

   }
}