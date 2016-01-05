using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models
{
    public class EpitesViewModel
    {
        

        public List<EpitesEpuletListViewModel> Epuletek { get; set; }

    }

    public class EpitesEpuletListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Ido { get; set; }

        public int Hatravan { get; set; }

        public int JelenlegVan { get; set; }


    }
}