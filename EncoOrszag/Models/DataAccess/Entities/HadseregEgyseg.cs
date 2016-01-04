using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class HadseregEgyseg
    {
        public int Id { get; set; }

        public int Darab { get; set; }

        public virtual Egyseg Egyseg { get; set; }

        public virtual Hadsereg Hadsereg { get; set; }

    }
}