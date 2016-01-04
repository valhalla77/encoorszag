using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class Egyseg
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Tamadas { get; set; }

        public int Vedekezes { get; set; }

        public int Ar { get; set; }

        public int Zsold { get; set; }

        public int Ellatmany { get; set; }

        public virtual ICollection<OrszagEgyseg> OrszagEgysegek { get; set; }

        public virtual ICollection<HadseregEgyseg> HadseregEgysegek { get; set; }
    }
}