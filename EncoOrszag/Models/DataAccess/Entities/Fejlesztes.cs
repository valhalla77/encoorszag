using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class Fejlesztes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Ido { get; set; }

        public virtual ICollection<OrszagFejlesztes> OrszagFejlesztesek { get; set; }

        public virtual ICollection<OrszagFejlesztesKeszul> OrszagFejlesztesKeszulesek { get; set; }
    }
}