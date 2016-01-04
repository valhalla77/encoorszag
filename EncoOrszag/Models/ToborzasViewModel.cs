using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models
{
    public class ToborzasViewModel
    {
        public int Szabadhelyek { get; set; }

        public int Arany { get; set; }

        public  List<ToborzasEgysegListViewModel> Egysegek { get; set; }
    }

    public class ToborzasEgysegListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Tamadas { get; set; }

        public int Vedekezes { get; set; }

        public int Ar { get; set; }

        public int Zsold { get; set; }

        public int Ellatmany { get; set; }

        public int JelenlegVan { get; set; }

        public int Vetel { get; set; }
    }
}