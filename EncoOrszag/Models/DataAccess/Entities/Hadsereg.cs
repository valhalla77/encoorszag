using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models.DataAccess.Entities
{
    public class Hadsereg
    {
        public int Id { get; set; }

        [InverseProperty("SajatHadseregek")]
        public virtual Orszag SajatOrszag{ get; set; }

        [InverseProperty("EllensegHadseregek")]
        public virtual Orszag CelOrszag { get; set; }

        public virtual ICollection<HadseregEgyseg> HadseregEgysegek  { get; set; }
    }
}