using EncoOrszag.Models.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncoOrszag.Models
{
   public class HaboruViewModel
   {
      public List<HaboruEgysegListViewModel> JelenlegiEgysegek { get; set; }

      public List<SelectListItem> Orszagok { get; set; }

      public List<HaboruHadseregListViewModel> Hadseregek { get; set; }

   }

   public class HaboruEgysegListViewModel
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public int Tamadas { get; set; }

      public int Vedekezes { get; set; }

      public int OsszesenVan { get; set; }

      public int JelenlegVan { get; set; }
   }

   public class HaboruHadseregListViewModel
   {
      public int Id { get; set; }

      public string CelOrszag { get; set; }

      public int CelOrszagId { get; set; }

      public List<HadseregEgysegListViewModel> HadseregEgysegek { get; set; }
   }

   public class HadseregEgysegListViewModel
   {
      public int Id { get; set; }

      public string Egyseg { get; set; }

      public int Darab { get; set; }
   }


}