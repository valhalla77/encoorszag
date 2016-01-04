using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EncoOrszag.Models.DataAccess.Entities
{
    public class Orszag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Pontszam { get; set; }

        public int Krumpli { get; set; }

        public int Arany { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrszagEpulet> OrszagEpuletek { get; set; }

        public virtual ICollection<OrszagEgyseg> OrszagEgysegek { get; set; }

        public virtual ICollection<OrszagEpuletKeszul> OrszagEpuletKeszulesek { get; set; }

        public virtual ICollection<OrszagFejlesztes> OrszagFejlesztesek { get; set; }

        public virtual ICollection<OrszagFejlesztesKeszul> OrszagFejlesztesKeszulesek { get; set; }

        public virtual ICollection<Hadsereg> SajatHadseregek { get; set; }

        public virtual ICollection<Hadsereg> EllensegHadseregek { get; set; }
        
    }
}