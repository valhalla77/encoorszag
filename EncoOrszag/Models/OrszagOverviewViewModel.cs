using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models
{
    public class OrszagOverviewViewModel
    {
        public string Name { get; set; }

        public int Pontszam { get; set; }

        public int Krumpli { get; set; }

        public int Arany { get; set; }

        public int Nepesseg { get; set; }

        public int BevetelPerKor { get; set; }

        public int SzabadHelyek { get; set; }

        public int ZsoldPerKor { get; set; }

        public int EllatmanyperKor { get; set; }

        public Dictionary<string , int> Epuletek { get; set; }

        public Dictionary<string, int> Epitesalatt { get; set; }

        public Dictionary<string, int> Egysegek { get; set; }

        public Dictionary<string, bool> Fejlesztesek { get; set; }

        public Dictionary<string, int> Felesztesalatt { get; set; }

    }
}