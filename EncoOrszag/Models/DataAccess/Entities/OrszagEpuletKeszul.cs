using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class OrszagEpuletKeszul
    {
        public int Id { get; set; }

        public int Hatravan { get; set; }

        public virtual Orszag Orszag { get; set; }

        public virtual Epulet Epulet { get; set; }



    }
}