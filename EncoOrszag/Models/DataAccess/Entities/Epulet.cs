using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class Epulet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Ido { get; set; }

        public virtual ICollection<OrszagEpulet> OrszagEpuletek { get; set; }

        public virtual ICollection<OrszagEpuletKeszul> OrszagEpuletKeszulesek { get; set; }


    }
}