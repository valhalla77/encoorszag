using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class OrszagEgyseg
    {
        public int Id { get; set; }

        public int Darab { get; set; }

        public virtual Orszag Orszag { get; set; }

        public virtual Egyseg Egyseg { get; set; }
    }
}