using EncoOrszag.Models.DataAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EncoOrszag.Models.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Egyseg> Egysegek { get; set; }

        public DbSet<Epulet> Epuletek { get; set; }

        public DbSet<Fejlesztes> Fejlesztesek { get; set; }

        public DbSet<Hadsereg> Hadseregek { get; set; }

        public DbSet<HadseregEgyseg> HadseregEgysegek { get; set; }

        public DbSet<Orszag> Orszagok { get; set; }

        public DbSet<OrszagEgyseg> OrszagEgysegek { get; set; }

        public DbSet<OrszagEpulet> OrszagEpuletek { get; set; }

        public DbSet<OrszagEpuletKeszul> OrszagEpuletKeszulesek { get; set; }

        public DbSet<OrszagFejlesztes> OrszagFejlesztesek { get; set; }

        public DbSet<OrszagFejlesztesKeszul> OrszagFejlesztesKeszulesek { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}